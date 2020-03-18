namespace Checkout.Identity.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "identity";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Identity
        {
            public const string Register = Base + "/register";

            public const string Login = Base + "/login";

            public const string RefreshToken = Base + "/refreshtoken";
        }
    }
}