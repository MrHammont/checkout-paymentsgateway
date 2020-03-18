using System;

namespace Checkout.PaymentsGateway.Domain.Models
{
    public class TransactionResult
    {
        public Guid TransactionId { get; set; }
        public string TransactionStatus { get; set; }
    }
}