using Checkout.PaymentsGateway.Api.Utils;

namespace Checkout.PaymentsGateway.Api.Cache
{
    public class CacheKeyProvider : ICacheKeyProvider
    {
        private readonly IStringEncoder _stringEncoder;

        public CacheKeyProvider(IStringEncoder stringEncoder)
        {
            _stringEncoder = stringEncoder;
        }

        public string GenerateCacheKeyFromRequest(params string[] args)
        {
            if (args.Length == 0)
                return string.Empty;

            var key = string.Join("_", args);
            key = _stringEncoder.Encode(key);

            return key;
        }
    }
}