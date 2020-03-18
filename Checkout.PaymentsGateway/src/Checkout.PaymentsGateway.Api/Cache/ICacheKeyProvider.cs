namespace Checkout.PaymentsGateway.Api.Cache
{
    public interface ICacheKeyProvider
    {
        string GenerateCacheKeyFromRequest(params string[] args);
    }
}