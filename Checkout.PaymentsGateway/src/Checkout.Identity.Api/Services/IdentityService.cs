using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Identity.Api.Data;
using Checkout.Identity.Api.Models;
using Checkout.Identity.Api.Providers;
using Checkout.Identity.Auth.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Identity.Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenProvider _tokenProvider;
        private readonly IPrincipalProvider _principalProvider;
        private readonly JwtSettings _jwtSettings;
        private readonly IdentityDb _context;

        public IdentityService(UserManager<IdentityUser> userManager, ITokenProvider tokenProvider,
            IPrincipalProvider principalProvider, JwtSettings jwtSettings, IdentityDb context)
        {
            _userManager = userManager;
            _tokenProvider = tokenProvider;
            _principalProvider = principalProvider;
            _jwtSettings = jwtSettings;
            _context = context;
        }

        public async Task<AuthenticationResponse> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return UserPasswordInvalidResponse();

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword) return UserPasswordInvalidResponse();

            var token = await _tokenProvider.GetTokenForUserAsync(user);

            return new AuthenticationResponse
            {
                Success = true,
                Token = token.Token,
                RefreshToken = token.Refresh
            };
        }

        public async Task<AuthenticationResponse> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = _principalProvider.GetPrincipalFromToken(token);

            if (validatedToken == null) return InvalidTokenResponse();

            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow) return InvalidTokenResponse();

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (storedRefreshToken == null) return InvalidTokenResponse();

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate) return InvalidTokenResponse();

            if (storedRefreshToken.Invalidated) return InvalidTokenResponse();

            if (storedRefreshToken.Used) return InvalidTokenResponse();

            if (storedRefreshToken.JwtId != jti) return InvalidTokenResponse();

            storedRefreshToken.Used = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);

            var userToken = await _tokenProvider.GetTokenForUserAsync(user);

            return new AuthenticationResponse
            {
                Success = true,
                Token = userToken.Token,
                RefreshToken = userToken.Refresh
            };
        }

        public async Task<AuthenticationResponse> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null) return new AuthenticationResponse {Errors = new[] {"Email already in use"}};

            var newUserId = Guid.NewGuid();
            var user = new IdentityUser
            {
                Id = newUserId.ToString(),
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(user, password);

            if (!createdUser.Succeeded)
                return new AuthenticationResponse {Errors = createdUser.Errors.Select(x => x.Description)};

            var token = await _tokenProvider.GetTokenForUserAsync(user);

            return new AuthenticationResponse
            {
                Success = true,
                Token = token.Token,
                RefreshToken = token.Refresh
            };
        }

        private AuthenticationResponse UserPasswordInvalidResponse()
        {
            return new AuthenticationResponse()
            {
                Errors = new[] {"User/password combination is not correct"}
            };
        }

        private AuthenticationResponse InvalidTokenResponse()
        {
            return new AuthenticationResponse()
            {
                Errors = new[] {"Invalid token error"}
            };
        }
    }
}