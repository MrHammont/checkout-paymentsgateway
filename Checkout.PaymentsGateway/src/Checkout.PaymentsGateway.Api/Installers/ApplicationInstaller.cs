using System.Reflection;
using Checkout.PaymentsGateway.Api.Extensions;
using Checkout.PaymentsGateway.Api.Filters;
using Checkout.PaymentsGateway.Api.Handlers;
using Checkout.PaymentsGateway.Api.Proxy;
using Checkout.PaymentsGateway.Api.Services;
using Checkout.PaymentsGateway.Api.Utils;
using Checkout.PaymentsGateway.Domain.Repositories;
using Checkout.PaymentsGateway.Domain.Services;
using Checkout.PaymentsGateway.Infrastructure.Instrumentors;
using Checkout.PaymentsGateway.Infrastructure.Repositories;
using Checkout.PaymentsGateway.Infrastructure.Utils;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Checkout.PaymentsGateway.Api.Installers
{
    public class ApplicationInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMvc(options => { options.Filters.Add<ValidationFilter>(); })
                .AddFluentValidation(fv =>
                    fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(Startup))));

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddSingleton<IUriService>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
                return new UriService(absoluteUri);
            });

            services.AddSingleton<JsonSerializer>();

            services.AddSingleton<IBankRepository, BankRepository>();
            services.Decorate<IBankRepository, InstrumentedBankRepository>();

            services.AddScoped<IPaymentsRepository, PaymentsRepository>();
            services.Decorate<IPaymentsRepository, PaymentsEncryptorRepository>();
            services.Decorate<IPaymentsRepository, InstrumentedPaymentsRepository>();

            services.AddScoped<ICreatePaymentService, CreatePaymentService>();
            services.AddScoped<IGetPaymentService, GetPaymentService>();
            services.Decorate<IGetPaymentService, GetMaskedPaymentService>();


            services.AddSingleton<IGetPaymentResponseMasker, GetPaymentResponseMasker>();

            services.AddSingleton<IEncryptor, Encryptor>();

            services.AddSingleton<IInstrumentor, Instrumentor>();

            services.AddSingleton<IGlobalExceptionHandler, GlobalExceptionHandler>();
            services.AddSingleton<IExceptionWriter, ExceptionWriter>();
        }
    }
}