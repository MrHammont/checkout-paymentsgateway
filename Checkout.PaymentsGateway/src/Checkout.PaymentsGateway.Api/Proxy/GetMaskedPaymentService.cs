using System;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.Api.Utils;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Domain.Services;

namespace Checkout.PaymentsGateway.Api.Proxy
{
    public class GetMaskedPaymentService : IGetPaymentService
    {
        private readonly IGetPaymentService _innerPaymentService;
        private readonly IGetPaymentResponseMasker _masker;

        public GetMaskedPaymentService(IGetPaymentService innerPaymentService, IGetPaymentResponseMasker masker)
        {
            _innerPaymentService = innerPaymentService;
            _masker = masker;
        }

        public async Task<PaymentRecord?> GetPaymentAsync(Guid paymentId, Guid companyId)
        {
            var result = await _innerPaymentService.GetPaymentAsync(paymentId, companyId);

            if (result != null)
                result = _masker.MaskPaymentRecord(result);

            return result;
        }
    }
}