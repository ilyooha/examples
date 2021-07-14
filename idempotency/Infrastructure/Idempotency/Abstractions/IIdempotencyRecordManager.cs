using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Idempotency.Abstractions
{
    public interface IIdempotencyRecordManager
    {
        public Task<IIdempotencyRecord> Get(string scope, string idempotencyKey, CancellationToken cancel);
        public Task Save(string scope, string idempotencyKey, string result, CancellationToken cancel);
    }
}