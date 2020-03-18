using System;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.PaymentsGateway.BankIntegration.Contracts.V1.Requests;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Domain.Repositories;
using Checkout.PaymentsGateway.Infrastructure.HttpClients;

namespace Checkout.PaymentsGateway.Infrastructure.Repositories
{
    public class BankRepository : IBankRepository
    {
        private readonly IBankHttpClient _bankHttpClient;
        private readonly IMapper _mapper;

        public BankRepository(IBankHttpClient bankHttpClient, IMapper mapper)
        {
            _bankHttpClient = bankHttpClient;
            _mapper = mapper;
        }

        public async Task<TransactionResult> CreateTransactionAsync(BankTransaction transaction)
        {
            var request = _mapper.Map<CreateBankTransactionRequest>(transaction);

            var response = await _bankHttpClient.CreateTransactionAsync(request);

            var transactionResult = new TransactionResult
            {
                TransactionId = new Guid(response.TransactionId),
                TransactionStatus = response.Status
            };

            return transactionResult;
        }
    }
}