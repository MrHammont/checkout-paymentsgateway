using System.Threading.Tasks;
using Checkout.Identity.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Checkout.Identity.Api.Providers
{
    public interface ITokenProvider
    {
        Task<RefreshToken> GetTokenForUserAsync(IdentityUser user);
    }
}