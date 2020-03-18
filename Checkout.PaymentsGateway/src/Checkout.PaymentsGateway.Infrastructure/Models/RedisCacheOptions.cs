namespace Checkout.PaymentsGateway.Infrastructure.Models
{
    public class RedisCacheOptions
    {
        public string ConnectionString { get; set; }
        public bool Enabled { get; set; }
    }
}