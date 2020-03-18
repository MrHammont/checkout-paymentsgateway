using Checkout.PaymentsGateway.Domain.Models;

namespace Checkout.PaymentsGateway.Api.Utils
{
    public interface IGetPaymentResponseMasker
    {
        PaymentRecord MaskPaymentRecord(PaymentRecord paymentRecord);
    }
}