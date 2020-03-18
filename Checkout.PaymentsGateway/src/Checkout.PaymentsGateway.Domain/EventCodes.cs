using System.Diagnostics.Tracing;
using Checkout.Core.Logging;

namespace Checkout.PaymentsGateway.Domain
{
    public class EventCodes
    {
        private const string Prefix = "PAYMENTSGATEWAY";

        public static readonly string PaymentRequestedNotFound =
            BaseEventCodes.EventCode(EventLevel.Warning, "PaymentRequestedNotFound", Prefix);

        public static readonly string BankTransactionCreated =
            BaseEventCodes.EventCode(EventLevel.Warning, "BankTransactionCreated", Prefix);
    }
}