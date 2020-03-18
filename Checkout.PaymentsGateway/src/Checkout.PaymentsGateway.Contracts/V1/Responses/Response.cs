namespace Checkout.PaymentsGateway.Contracts.V1.Responses
{
    public class Response<T>
    {
        public T Data { get; set; }
        public string Status { get; set; }
    }
}