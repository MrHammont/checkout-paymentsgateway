using System;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.Domain.Models;

namespace Checkout.PaymentsGateway.Domain.Services
{
    public interface IGetPaymentService
    {
        Task<PaymentRecord?> GetPaymentAsync(Guid paymentId, Guid companyId);
    }
}