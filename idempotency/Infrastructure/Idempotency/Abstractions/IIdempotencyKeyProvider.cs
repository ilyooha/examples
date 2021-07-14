using System.Threading.Tasks;

namespace Infrastructure.Idempotency.Abstractions
{
    public interface IIdempotencyKeyProvider
    {
        Task<string> Get();
    }
}