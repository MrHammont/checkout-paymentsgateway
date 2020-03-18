using System;
using Checkout.PaymentsGateway.Api.Middlewares;
using Checkout.PaymentsGateway.Api.Options;
using Microsoft.AspNetCore.Builder;

namespace Checkout.PaymentsGateway.Api.Extensions
{
    public static class CorrelationIdExtensions
    {
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app, CorrelationIdOptions options)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            if (options == null) throw new ArgumentNullException(nameof(options));

            return app.UseMiddleware<CorrelationIdMiddleware>(options);
        }
    }
}