using System.Net;
using System.Threading.Tasks;
using Checkout.Core.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentsGateway.Api.Handlers
{
    public class GlobalExceptionHandler : IGlobalExceptionHandler
    {
        private readonly IExceptionWriter _exceptionWriter;
        private readonly IAppLogger _logger;

        public GlobalExceptionHandler(IExceptionWriter exceptionWriter, IAppLogger logger)
        {
            _exceptionWriter = exceptionWriter;
            _logger = logger;
        }

        public async Task Handle(HttpContext context)
        {
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                _logger.Write(LogLevel.Error, $"{EventCodes.InternalServerError} - {contextFeature.Error.Message}");

                await _exceptionWriter.WriteResponse(context);
            }
        }
    }
}