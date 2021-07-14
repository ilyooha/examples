using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Idempotency.Abstractions;
using MediatR;

namespace Infrastructure.Idempotency.MediatR
{
    public class IdempotencyBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    {
        private readonly IIdempotencyKeyProvider _idempotencyKeyProvider;
        private readonly IIdempotencyRecordManager _idempotencyRecordProvider;

        public IdempotencyBehavior(IIdempotencyRecordManager idempotencyRecordProvider,
            IIdempotencyKeyProvider idempotencyKeyProvider)
        {
            _idempotencyRecordProvider = idempotencyRecordProvider;
            _idempotencyKeyProvider = idempotencyKeyProvider;
        }

        public async Task<TResult> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResult> next)
        {
            // Невозможно просто добавить 'where TRequest: IIdempotentCommand<TResult>' к описанию типа,
            // т. к. резолв PipelineBehavior не учитывает constraint'ов. Это приведёт к исключению в рантайме,
            // поэтому приходится ставить фильтр по типу запроса и игнорировать неподходящие.
            //
            // ВНИМАНИЕ! при использовании IRequest<TResult> и IIdempotentCommand<TResult>
            // TResult должен быть одинаковым
            if (!(request is IIdempotentCommand<TResult> command))
            {
                return await next();
            }

            var idempotencyKey = await _idempotencyKeyProvider.Get();
            if (idempotencyKey == null)
            {
                return await next();
            }

            var idempotencyRecord =
                await _idempotencyRecordProvider.Get(command.CommandTypeId, idempotencyKey, cancellationToken);

            if (idempotencyRecord != null)
            {
                var prevResult = command.DeserializeResult(idempotencyRecord.Result);
                return prevResult;
            }

            var result = await next();

            var resultSerialized = command.SerializeResult(result);

            await _idempotencyRecordProvider.Save(command.CommandTypeId, idempotencyKey, resultSerialized,
                cancellationToken);

            return result;
        }
    }
}