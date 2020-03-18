using AutoMapper;
using Checkout.Core.Logging;
using Checkout.PaymentsGateway.Api.Extensions;
using Checkout.PaymentsGateway.Api.Middlewares;
using Checkout.PaymentsGateway.Api.Options;
using Checkout.PaymentsGateway.Api.Utils;
using Checkout.PaymentsGateway.DataContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Checkout.PaymentsGateway.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAppLogger, Logger>();
            services.InstallServicesInAssembly<Startup>(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.AddDataProtection();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.SeedData<PaymentsDb>();
            }

            app.ConfigureExceptionHandler();

            var correlationIdOptions = new CorrelationIdOptions();
            Configuration.GetSection(nameof(CorrelationIdOptions)).Bind(correlationIdOptions);
            app.UseCorrelationId(correlationIdOptions);

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            app.UseMySwagger(swaggerOptions);

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}