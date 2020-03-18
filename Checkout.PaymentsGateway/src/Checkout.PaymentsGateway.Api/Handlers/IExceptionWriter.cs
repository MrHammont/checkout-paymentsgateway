using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Checkout.PaymentsGateway.Api.Handlers
{
    public interface IExceptionWriter
    {
        Task WriteResponse(HttpContext context);
    }
}