using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Checkout.PaymentsGateway.Api.Handlers
{
    public interface IGlobalExceptionHandler
    {
        Task Handle(HttpContext context);
    }
}