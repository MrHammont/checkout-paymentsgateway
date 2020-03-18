using System;
using System.Threading.Tasks;
using Checkout.Core.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Checkout.PaymentsGateway.Api.Cache
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IAppLogger _logger;

        public ResponseCacheService(IDistributedCache distributedCache, IAppLogger logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (string.IsNullOrEmpty(cacheKey))
            {
                _logger.Write(LogLevel.Warning, EventCodes.CacheKeyMissingInCacheRequest);
                return;
            }

            if (response == null)
            {
                _logger.Write(LogLevel.Warning, EventCodes.CacheObjectMissingInCacheRequest);
                return;
            }

            var serializedResponse = JsonConvert.SerializeObject(response);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            };

            await _distributedCache.SetStringAsync(cacheKey, serializedResponse, cacheOptions);
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);

            return cachedResponse;
        }
    }
}