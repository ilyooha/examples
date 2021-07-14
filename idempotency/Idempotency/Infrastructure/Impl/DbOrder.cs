using System;
using Idempotency.Services.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Idempotency.Infrastructure.Impl
{
    public class DbOrder : IEntityTypeConfiguration<DbOrder>, IOrder
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        public void Configure(EntityTypeBuilder<DbOrder> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Orders");
            builder.Property(x => x.Description)
                .HasMaxLength(512);
        }
    }
}