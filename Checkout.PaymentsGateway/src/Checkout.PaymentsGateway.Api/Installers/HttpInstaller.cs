using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Checkout.Core.Logging;
using Checkout.PaymentsGateway.Infrastructure.Handlers;
using Checkout.PaymentsGateway.Infrastructure.HttpClients;
using Checkout.PaymentsGateway.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace Checkout.PaymentsGateway.Api.Installers
{
    public class HttpInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var bankClientOptions = new BankClientOptions();
            configuration.Bind(nameof(bankClientOptions), bankClientOptions);
            services.AddSingleton(bankClientOptions);

            services.AddTransient<HttpExceptionHandler>();

            var logger = services.BuildServiceProvider().GetService<IAppLogger>();
            services.AddHttpClient<IBankHttpClient, BankHttpClient>(x =>
                {
                    x.Timeout = TimeSpan.FromSeconds(bankClientOptions.Timeout);
                    x.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    x.BaseAddress = new Uri(bankClientOptions.BaseUrl);
                })
                .AddHttpMessageHandler<HttpExceptionHandler>()
                .AddPolicyHandler(GetRetryPolicy(logger, bankClientOptions));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IAppLogger logger, BankClientOptions options)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<OperationCanceledException>()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(
                    options.RetryCount,
                    retryAttempt => TimeSpan.FromSeconds(options.RetryWait),
                    (result, span) =>
                    {
                        logger.Write(LogLevel.Warning, $"{EventCodes.BankApiRetry} - {result.Exception.Message}");
                    });
        }
    }
}