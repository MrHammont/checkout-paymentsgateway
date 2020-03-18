namespace Checkout.PaymentsGateway.Contracts.V1.Responses
{
    public class CardDetails
    {
        public string HolderName { get; set; }
        public string Number { get; set; }
        public string ExpirationDate { get; set; }
    }
}