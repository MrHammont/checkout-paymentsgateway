using System;
using Checkout.PaymentsGateway.Contracts.V1;

namespace Checkout.PaymentsGateway.Api.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPaymentUri(Guid paymentId)
        {
            return new Uri(_baseUri + ApiRoutes.Payments.Get.Replace("{id}", paymentId.ToString()));
        }
    }
}