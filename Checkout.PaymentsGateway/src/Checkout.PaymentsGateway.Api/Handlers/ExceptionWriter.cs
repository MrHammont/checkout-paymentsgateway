using System.Threading.Tasks;
using Checkout.PaymentsGateway.Contracts.V1.Responses;
using Microsoft.AspNetCore.Http;

namespace Checkout.PaymentsGateway.Api.Handlers
{
    public class ExceptionWriter : IExceptionWriter
    {
        public async Task WriteResponse(HttpContext context)
        {
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error."
            }.ToString());
        }
    }
}