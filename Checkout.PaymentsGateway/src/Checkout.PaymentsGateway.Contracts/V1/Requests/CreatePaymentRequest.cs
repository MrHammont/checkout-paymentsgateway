namespace Checkout.PaymentsGateway.Contracts.V1.Requests
{
    public class CreatePaymentRequest
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationMonth { get; set; }
        public string CardExpirationYear { get; set; }
        public string Cvv { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}