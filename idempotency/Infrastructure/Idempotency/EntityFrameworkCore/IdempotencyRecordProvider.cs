using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Idempotency.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Idempotency.EntityFrameworkCore
{
    public class IdempotencyRecordManager : IIdempotencyRecordManager
    {
        private readonly DbContext _dbContext;
        private readonly IUserIdProvider _userIdProvider;

        public IdempotencyRecordManager(DbContext dbContext, IUserIdProvider userIdProvider)
        {
            _dbContext = dbContext;
            _userIdProvider = userIdProvider;
        }

        public async Task<IIdempotencyRecord> Get(string scope, string idempotencyKey, CancellationToken cancel)
        {
            var userId = await GetCurrentUserId();
            var record = await _dbContext.Set<DbIdempotencyRecord>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId.Equals(userId) &&
                                          x.Scope.Equals(scope) &&
                                          x.IdempotencyKey.Equals(idempotencyKey), cancel);

            return record;
        }

        public async Task Save(string scope, string idempotencyKey, string result, CancellationToken cancel)
        {
            var userId = await GetCurrentUserId();
            var record = new DbIdempotencyRecord
            {
                UserId = userId,
                Scope = scope,
                IdempotencyKey = idempotencyKey,
                Result = result,
                TimestampUtc = DateTime.UtcNow
            };

            await _dbContext.Set<DbIdempotencyRecord>().AddAsync(record, cancel);
        }

        private async Task<string> GetCurrentUserId()
        {
            var userId = await _userIdProvider.GetCurrentUserId();
            return userId;
        }
    }
}