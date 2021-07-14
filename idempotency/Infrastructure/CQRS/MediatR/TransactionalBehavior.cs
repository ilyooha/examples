using System.Threading;
using System.Threading.Tasks;
using Infrastructure.CQRS.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.CQRS.MediatR
{
    public class TransactionalBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly DbContext _dbContext;

        public TransactionalBehavior(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            // Невозможно просто добавить 'where TRequest: ICommand' к описанию типа,
            // т. к. резолв PipelineBehavior не учитывает constraint'ов. Это приведёт к исключению в рантайме,
            // поэтому приходится ставить фильтр по типу запроса и игнорировать неподходящие. 
            if (!(request is ICommand))
            {
                return await next();
            }

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var response = await next();

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return response;
        }
    }
}