using System;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.Domain.Models;

namespace Checkout.PaymentsGateway.Domain.Repositories
{
    public interface IPaymentsRepository
    {
        Task AddPaymentAsync(PaymentRecord paymentRecord);
        Task<PaymentRecord?> GetPaymentAsync(Guid paymentId, Guid companyId);
    }
}