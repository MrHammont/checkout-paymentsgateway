using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Checkout.PaymentsGateway.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetCompanyId(this HttpContext httpContext)
        {
            if (httpContext.User == null) return Guid.Empty;

            var id = httpContext.User.Claims.Single(x => x.Type == "id").Value;

            return new Guid(id);
        }
    }
}