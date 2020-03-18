namespace Checkout.PaymentsGateway.Api.Options
{
    public class PaymentsDbOptions
    {
        public string ConnectionString { get; set; }
        public int RetryCount { get; set; }
        public int RetryDelay { get; set; }
        public int Timeout { get; set; }
    }
}