using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Infrastructure.Idempotency.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Idempotency.AspNetCore
{
    public class HttpContextUserIdProvider : IUserIdProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextUserIdProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Task<string> GetCurrentUserId()
        {
            var user = _contextAccessor.HttpContext.User;
            var userId = "anonymous";

            if (user.Identity.IsAuthenticated)
            {
                userId = user.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            }

            return Task.FromResult(userId);
        }
    }
}