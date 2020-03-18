using System;

namespace Checkout.PaymentsGateway.Contracts.V1.Events
{
    public class TransactionCreatedEvent
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public string CardExpirationMonth { get; set; }
        public string CardExpirationYear { get; set; }
        public string Cvv { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string TransactionStatus { get; set; }
    }
}