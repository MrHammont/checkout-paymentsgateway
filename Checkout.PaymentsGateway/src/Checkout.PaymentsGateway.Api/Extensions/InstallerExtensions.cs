using System;
using System.Linq;
using Checkout.PaymentsGateway.Api.Installers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentsGateway.Api.Extensions
{
    public static class InstallerExtensions
    {
        public static void InstallServicesInAssembly<T>(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(T).Assembly.ExportedTypes
                .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}