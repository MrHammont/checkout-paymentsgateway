using System;

namespace Checkout.PaymentsGateway.Contracts.V1.Responses
{
    public class GetPaymentResponse
    {
        public Guid TransactionId { get; set; }
        public CardDetails CardDetails { get; set; }
        public TransactionDetails TransactionDetails { get; set; }
    }
}