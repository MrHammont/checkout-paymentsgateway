namespace Checkout.PaymentsGateway.Contracts.V1.Responses
{
    public class CreatePaymentResponse
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
    }
}