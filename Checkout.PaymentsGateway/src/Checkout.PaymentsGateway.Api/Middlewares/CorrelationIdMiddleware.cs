using System;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.Api.Options;
using Microsoft.AspNetCore.Http;

namespace Checkout.PaymentsGateway.Api.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CorrelationIdOptions _options;

        public CorrelationIdMiddleware(RequestDelegate next, CorrelationIdOptions options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(_options.Header, out var correlationId))
                context.TraceIdentifier = correlationId;
            else
                context.TraceIdentifier = Guid.NewGuid().ToString();

            if (_options.IncludeInResponse)
                context.Response.OnStarting((o) =>
                {
                    context.Response.Headers.Add(_options.Header, new[] {context.TraceIdentifier});
                    return Task.CompletedTask;
                }, true);

            return _next(context);
        }
    }
}