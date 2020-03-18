using Checkout.PaymentsGateway.Api.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentsGateway.Api.Middlewares
{
    public static class ExceptionHandlerMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                var exceptionHandler = appError.ApplicationServices.GetService<IGlobalExceptionHandler>();
                appError.Run(async context =>
                    await exceptionHandler.Handle(context));
            });
        }
    }
}