using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simple.Application.WriteModel;
using Simple.Infrastructure;
using Simple.Infrastructure.Persistence;

namespace Simple
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(RegisterProductCommandHandler), typeof(ProductQueryHandler));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionalBehavior<,>));

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("app");
                options.ConfigureWarnings(builder => { builder.Ignore(InMemoryEventId.TransactionIgnoredWarning); });
            });
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddControllers();
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