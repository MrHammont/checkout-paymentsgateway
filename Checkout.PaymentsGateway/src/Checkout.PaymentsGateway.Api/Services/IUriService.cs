using System;

namespace Checkout.PaymentsGateway.Api.Services
{
    public interface IUriService
    {
        Uri GetPaymentUri(Guid paymentId);
    }
}