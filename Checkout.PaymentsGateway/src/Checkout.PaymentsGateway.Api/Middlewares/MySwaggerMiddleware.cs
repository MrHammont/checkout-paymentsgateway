using Checkout.PaymentsGateway.Api.Options;
using Microsoft.AspNetCore.Builder;

namespace Checkout.PaymentsGateway.Api.Middlewares
{
    public static class MySwaggerMiddleware
    {
        public static void UseMySwagger(this IApplicationBuilder app, SwaggerOptions swaggerOptions)
        {
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
            });
        }
    }
}