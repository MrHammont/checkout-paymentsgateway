using System;

namespace Checkout.PaymentsGateway.Contracts.V1.Responses
{
    public class TransactionDetails
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionStatus { get; set; }
    }
}