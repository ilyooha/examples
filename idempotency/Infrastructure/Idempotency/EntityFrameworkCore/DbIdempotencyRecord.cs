using System;
using Infrastructure.Idempotency.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Idempotency.EntityFrameworkCore
{
    public class DbIdempotencyRecord : IEntityTypeConfiguration<DbIdempotencyRecord>, IIdempotencyRecord
    {
        // userId | scope (operationId) | value (idempotency key) | result
        public string UserId { get; set; }
        public string Scope { get; set; }
        public string IdempotencyKey { get; set; }
        public string Result { get; set; }
        public DateTime TimestampUtc { get; set; }

        public void Configure(EntityTypeBuilder<DbIdempotencyRecord> builder)
        {
            builder.ToTable("IdempotencyRecords");
            builder.HasKey(x => new {x.UserId, x.Scope, x.IdempotencyKey});
            builder.Property(x => x.UserId).HasMaxLength(64);
            builder.Property(x => x.Scope).HasMaxLength(128);
            builder.Property(x => x.IdempotencyKey).HasMaxLength(64);
            builder.Ignore(x => x.TimestampUtc);
        }
    }
}