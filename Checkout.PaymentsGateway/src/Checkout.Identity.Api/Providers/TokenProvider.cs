using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Checkout.Identity.Api.Data;
using Checkout.Identity.Api.Data.Dto;
using Checkout.Identity.Api.Models;
using Checkout.Identity.Auth.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Checkout.Identity.Api.Providers
{
    public class TokenProvider : ITokenProvider
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IdentityDb _context;

        public TokenProvider(JwtSettings jwtSettings, IdentityDb context)
        {
            _jwtSettings = jwtSettings;
            _context = context;
        }

        public async Task<RefreshToken> GetTokenForUserAsync(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshTokenDto
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.Add(_jwtSettings.RefreshTokenLifeTime),
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new RefreshToken
            {
                Token = tokenHandler.WriteToken(token),
                Refresh = refreshToken.Token
            };
        }
    }
}