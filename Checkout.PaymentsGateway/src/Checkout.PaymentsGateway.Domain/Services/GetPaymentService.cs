using System;
using System.Threading.Tasks;
using Checkout.Core.Logging;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentsGateway.Domain.Services
{
    public class GetPaymentService : IGetPaymentService
    {
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IAppLogger _logger;

        public GetPaymentService(IPaymentsRepository paymentsRepository, IAppLogger logger)
        {
            _paymentsRepository = paymentsRepository;
            _logger = logger;
        }

        public async Task<PaymentRecord?> GetPaymentAsync(Guid paymentId, Guid companyId)
        {
            var payment = await _paymentsRepository.GetPaymentAsync(paymentId, companyId);

            if (payment == null)
                _logger.Write(LogLevel.Warning,
                    $"{EventCodes.PaymentRequestedNotFound} - TransactionId: {paymentId}, CompanyId: {companyId}");

            return payment;
        }
    }
}