using Idempotency.Infrastructure.Impl;
using Infrastructure.CQRS.MediatR;
using Infrastructure.Idempotency.Abstractions;
using Infrastructure.Idempotency.AspNetCore;
using Infrastructure.Idempotency.EntityFrameworkCore;
using Infrastructure.Idempotency.MediatR;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Idempotency
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(builder =>
            {
                builder.UseInMemoryDatabase("orders");
                builder.ConfigureWarnings(config => { config.Ignore(InMemoryEventId.TransactionIgnoredWarning); });
            });
            services.AddScoped<DbContext>(provider => provider.GetService<AppDbContext>());

            services.AddControllers();

            services.AddMediatR(typeof(OrderRequestHandler));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionalBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(IdempotencyBehavior<,>));

            services.AddTransient<IIdempotencyRecordManager, IdempotencyRecordManager>();
            services.AddTransient<IIdempotencyKeyProvider, HttpContextIdempotencyKeyProvider>();

            services.AddHttpContextAccessor();
            services.AddScoped<IUserIdProvider, HttpContextUserIdProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}