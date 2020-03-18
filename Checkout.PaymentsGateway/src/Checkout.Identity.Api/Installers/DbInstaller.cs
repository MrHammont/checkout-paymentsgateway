using Checkout.Identity.Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.Identity.Api.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDb>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("IdentityDbConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<IdentityDb>();
        }
    }
}