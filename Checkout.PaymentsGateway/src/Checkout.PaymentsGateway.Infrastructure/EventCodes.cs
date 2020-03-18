using System.Diagnostics.Tracing;
using Checkout.Core.Logging;

namespace Checkout.PaymentsGateway.Infrastructure
{
    public class EventCodes
    {
        private const string Prefix = "PAYMENTSGATEWAY";

        public static string EventInstrumented(string instrumentorName)
        {
            return BaseEventCodes.EventCode(EventLevel.Informational, instrumentorName, Prefix);
        }

        public static string ErrorCallingBankApi =
            BaseEventCodes.EventCode(EventLevel.Error, "ErrorCallingBankApi", Prefix);

        public static string UnauthorizedCallToBankApi =
            BaseEventCodes.EventCode(EventLevel.Error, "UnauthorizedCallToBankApi", Prefix);
    }
}