using System.Diagnostics.Tracing;
using Checkout.Core.Logging;

namespace Checkout.PaymentsGateway.Api
{
    public class EventCodes
    {
        private const string Prefix = "PAYMENTSGATEWAY";

        public static readonly string InternalServerError =
            BaseEventCodes.EventCode(EventLevel.Error, "InternalServerError", Prefix);

        public static readonly string BankApiRetry =
            BaseEventCodes.EventCode(EventLevel.Warning, "BankRequestRetry", Prefix);

        public static readonly string CacheKeyMissingInCacheRequest =
            BaseEventCodes.EventCode(EventLevel.Warning, "CacheKeyMissingInCacheRequest", Prefix);

        public static readonly string CacheObjectMissingInCacheRequest =
            BaseEventCodes.EventCode(EventLevel.Warning, "CacheObjectMissingInCacheRequest", Prefix);
    }
}