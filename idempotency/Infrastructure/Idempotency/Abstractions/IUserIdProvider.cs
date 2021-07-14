using System.Threading.Tasks;

namespace Infrastructure.Idempotency.Abstractions
{
    public interface IUserIdProvider
    {
        public Task<string> GetCurrentUserId();
    }
}