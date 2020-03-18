namespace Checkout.PaymentsGateway.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "paymentsgateway";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Payments
        {
            public const string Get = Base + "/payments/{id}";

            public const string Post = Base + "/payments";
        }
    }
}