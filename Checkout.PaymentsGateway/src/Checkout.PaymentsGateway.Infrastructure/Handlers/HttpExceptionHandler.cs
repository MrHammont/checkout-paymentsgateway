using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Checkout.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentsGateway.Infrastructure.Handlers
{
    public class HttpExceptionHandler : DelegatingHandler
    {
        private readonly IAppLogger _logger;

        public HttpExceptionHandler(IAppLogger logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                return response;
            }
            catch (Exception e)
            {
                _logger.Write(LogLevel.Error, $"{EventCodes.ErrorCallingBankApi} - {e.Message}");
                throw;
            }
        }
    }
}