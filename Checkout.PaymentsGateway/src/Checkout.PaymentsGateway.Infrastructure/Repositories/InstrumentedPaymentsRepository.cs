using System;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Domain.Repositories;
using Checkout.PaymentsGateway.Infrastructure.Extensions;
using Checkout.PaymentsGateway.Infrastructure.Instrumentors;

namespace Checkout.PaymentsGateway.Infrastructure.Repositories
{
    public class InstrumentedPaymentsRepository : IPaymentsRepository
    {
        private readonly IPaymentsRepository _instrumentedPaymentsRepository;
        private readonly IInstrumentor _instrumentor;

        public InstrumentedPaymentsRepository(IPaymentsRepository instrumentedPaymentsRepository,
            IInstrumentor instrumentor)
        {
            _instrumentedPaymentsRepository = instrumentedPaymentsRepository;
            _instrumentor = instrumentor;
        }

        public async Task AddPaymentAsync(PaymentRecord paymentRecord)
        {
            await _instrumentor.ApplyAsync(
                () => _instrumentedPaymentsRepository.AddPaymentAsync(paymentRecord),
                "InstrumentedAddPaymentRepository");
        }

        public async Task<PaymentRecord?> GetPaymentAsync(Guid paymentId, Guid companyId)
        {
            var result = await _instrumentor.ApplyAsync(
                () => _instrumentedPaymentsRepository.GetPaymentAsync(paymentId, companyId),
                "InstrumentedGetPaymentRepository");

            return result;
        }
    }
}