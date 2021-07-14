using System.Threading.Tasks;
using Infrastructure.Idempotency.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Idempotency.AspNetCore
{
    public class HttpContextIdempotencyKeyProvider : IIdempotencyKeyProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextIdempotencyKeyProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Task<string> Get()
        {
            string value = null;

            if (_contextAccessor.HttpContext.Request.Headers.TryGetValue("Idempotency-Key", out var values))
            {
                value = values.ToString();
            }

            return Task.FromResult(value);
        }
    }
}