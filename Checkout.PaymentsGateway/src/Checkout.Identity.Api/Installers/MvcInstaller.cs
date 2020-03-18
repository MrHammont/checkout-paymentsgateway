using Checkout.Identity.Api.Providers;
using Checkout.Identity.Api.Services;
using Checkout.Identity.Auth.Auth;
using Checkout.Identity.Auth.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.Identity.Api.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddSingleton<IPrincipalProvider, PrincipalProvider>();

            services.AddMvc();

            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddJwtAuthentication(jwtSettings);
        }
    }
}