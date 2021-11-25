using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Simple.Application.WriteModel;
using Simple.Infrastructure.Persistence;

namespace Simple.Infrastructure
{
    public class TransactionalBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly AppDbContext _dbContext;

        public TransactionalBehavior(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            // if(request is a command) then do a transaction and save changes
            // else just forward the request to the handler

            if (request is not ICommand)
                return await next();

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            var response = await next();

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return response;
        }
    }
}