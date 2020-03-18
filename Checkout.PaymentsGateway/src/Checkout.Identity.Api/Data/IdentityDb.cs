using Checkout.Identity.Api.Data.Dto;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Identity.Api.Data
{
    public class IdentityDb : IdentityDbContext
    {
        public IdentityDb(DbContextOptions<IdentityDb> options)
            : base(options)
        {
        }

        public DbSet<RefreshTokenDto> RefreshTokens { get; set; }
    }
}