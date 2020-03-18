namespace Checkout.PaymentsGateway.Api.Options
{
    public class CorrelationIdOptions
    {
        private const string DefaultHeader = "X-Correlation-ID";

        public string Header { get; set; } = DefaultHeader;

        public bool IncludeInResponse { get; set; } = true;
    }
}