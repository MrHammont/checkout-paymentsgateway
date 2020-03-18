using System;
using Checkout.PaymentsGateway.Api.Options;
using Checkout.PaymentsGateway.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentsGateway.Api.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var paymentsDbOptions = new PaymentsDbOptions();
            configuration.Bind(nameof(paymentsDbOptions), paymentsDbOptions);

            services.AddDbContext<PaymentsDb>(options =>
            {
                options.UseSqlServer(paymentsDbOptions.ConnectionString,
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            paymentsDbOptions.RetryCount,
                            TimeSpan.FromSeconds(paymentsDbOptions.RetryDelay),
                            null);

                        sqlOptions.CommandTimeout(paymentsDbOptions.Timeout);
                    });
            });
        }
    }
}