using Checkout.PaymentsGateway.Api.Cache;
using Checkout.PaymentsGateway.Api.Utils;
using Checkout.PaymentsGateway.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Checkout.PaymentsGateway.Api.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = new RedisCacheOptions();
            configuration.GetSection(nameof(RedisCacheOptions)).Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);

            if (!redisCacheSettings.Enabled) return;

            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(redisCacheSettings.ConnectionString));

            services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);

            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddSingleton<ICacheKeyProvider, CacheKeyProvider>();
            services.AddSingleton<IStringEncoder, StringEncoder>();
        }
    }
}