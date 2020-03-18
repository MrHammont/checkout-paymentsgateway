using System.Threading.Tasks;
using Checkout.Identity.Api.Models;

namespace Checkout.Identity.Api.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResponse> RegisterAsync(string email, string password);

        Task<AuthenticationResponse> LoginAsync(string email, string password);

        Task<AuthenticationResponse> RefreshTokenAsync(string token, string refreshToken);
    }
}