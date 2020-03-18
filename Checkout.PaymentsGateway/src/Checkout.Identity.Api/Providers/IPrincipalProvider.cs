using System.Security.Claims;

namespace Checkout.Identity.Api.Providers
{
    public interface IPrincipalProvider
    {
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}