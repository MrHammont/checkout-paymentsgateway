namespace Checkout.PaymentsGateway.BankIntegration.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "bank";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Bank
        {
            public const string Post = Base + "/transaction";
        }
    }
}