namespace Checkout.PaymentsGateway.Infrastructure.Models
{
    public class BankClientOptions
    {
        public string BaseUrl { get; set; }
        public string Url { get; set; }

        public int RetryCount { get; set; }
        public int RetryWait { get; set; }
        public int Timeout { get; set; }
    }
}