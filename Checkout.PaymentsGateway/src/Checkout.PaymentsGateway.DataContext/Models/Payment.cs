using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.PaymentsGateway.DataContext.Models
{
    public class Payment
    {
        [Key] public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public DateTime CardExpirationDate { get; set; }
        public string Cvv { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string TransactionStatus { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}