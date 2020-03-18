using System;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Domain.Repositories;
using Checkout.PaymentsGateway.Infrastructure.Utils;

namespace Checkout.PaymentsGateway.Infrastructure.Repositories
{
    public class PaymentsEncryptorRepository : IPaymentsRepository
    {
        private readonly IPaymentsRepository _innerRepository;
        private readonly IEncryptor _encryptor;

        public PaymentsEncryptorRepository(IPaymentsRepository innerRepository, IEncryptor encryptor)
        {
            _innerRepository = innerRepository;
            _encryptor = encryptor;
        }

        public async Task AddPaymentAsync(PaymentRecord paymentRecord)
        {
            paymentRecord.CardNumber = _encryptor.Encrypt(paymentRecord.CardNumber);
            paymentRecord.CardName = _encryptor.Encrypt(paymentRecord.CardName);

            await _innerRepository.AddPaymentAsync(paymentRecord);
        }

        public async Task<PaymentRecord?> GetPaymentAsync(Guid paymentId, Guid companyId)
        {
            var innerResult = await _innerRepository.GetPaymentAsync(paymentId, companyId);

            if (innerResult != null)
            {
                innerResult.CardNumber = _encryptor.Decrypt(innerResult.CardNumber);
                innerResult.CardName = _encryptor.Decrypt(innerResult.CardName);
            }

            return innerResult;
        }
    }
}