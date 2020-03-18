using System;
using System.Threading.Tasks;
using Checkout.Core.Logging;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentsGateway.Domain.Services
{
    public class CreatePaymentService : ICreatePaymentService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IAppLogger _logger;

        public CreatePaymentService(IBankRepository bankRepository, IPaymentsRepository paymentsRepository,
            IAppLogger logger)
        {
            _bankRepository = bankRepository;
            _paymentsRepository = paymentsRepository;
            _logger = logger;
        }

        public async Task<TransactionResult> CreateTransaction(BankTransaction bankTransaction)
        {
            var transactionResult = await _bankRepository.CreateTransactionAsync(bankTransaction);

            _logger.Write(LogLevel.Information,
                $"{EventCodes.BankTransactionCreated} - TransactionId: {transactionResult.TransactionId}, TransactionStatus: {transactionResult.TransactionStatus}");

            var payment = GetPaymentRecordFromBankTransaction(transactionResult, bankTransaction);

            await _paymentsRepository.AddPaymentAsync(payment);

            return transactionResult;
        }

        private PaymentRecord GetPaymentRecordFromBankTransaction(TransactionResult bankTransactionResult,
            BankTransaction bankTransaction)
        {
            return new PaymentRecord()
            {
                Id = bankTransactionResult.TransactionId,
                CompanyId = bankTransaction.CompanyId,
                Amount = bankTransaction.Amount,
                CardExpirationMonth = bankTransaction.CardExpirationMonth,
                CardExpirationYear = bankTransaction.CardExpirationYear,
                CardName = bankTransaction.CardHolderName,
                CardNumber = bankTransaction.CardNumber,
                Currency = bankTransaction.Currency,
                Cvv = bankTransaction.Cvv,
                TransactionStatus = bankTransactionResult.TransactionStatus,
                TransactionDate = DateTime.UtcNow
            };
        }
    }
}