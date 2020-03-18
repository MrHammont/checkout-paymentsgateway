using System;
using System.Net.Http;
using System.Threading.Tasks;
using Checkout.Core.Logging;
using Checkout.PaymentsGateway.BankIntegration.Contracts.V1.Requests;
using Checkout.PaymentsGateway.BankIntegration.Contracts.V1.Responses;
using Checkout.PaymentsGateway.Infrastructure.Extensions;
using Checkout.PaymentsGateway.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Checkout.PaymentsGateway.Infrastructure.HttpClients
{
    public class BankHttpClient : IBankHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAppLogger _logger;
        private readonly JsonSerializer _jsonSerializer;
        private readonly BankClientOptions _bankOptions;

        public BankHttpClient(HttpClient httpClient,
            IAppLogger logger,
            JsonSerializer jsonSerializer,
            BankClientOptions bankOptions)
        {
            _httpClient = httpClient;
            _logger = logger;
            _jsonSerializer = jsonSerializer;
            _bankOptions = bankOptions;
        }

        public async Task<CreateBankTransactionResponse> CreateTransactionAsync(CreateBankTransactionRequest request)
        {
            var uri = new Uri(_bankOptions.Url);

            var transactionResponse = await _httpClient.PostAsJsonAsync(uri, request);

            await EnsureSuccessStatusAndLogAnyAuthErrors(transactionResponse);

            var response = await transactionResponse.Deserialize<CreateBankTransactionResponse>(_jsonSerializer);

            return response;
        }

        private async Task EnsureSuccessStatusAndLogAnyAuthErrors(HttpResponseMessage transactionResult)
        {
            try
            {
                await transactionResult.EnsureSuccessStatusCodeAsync();
            }
            catch (Exception ex)
            {
                if (transactionResult.IsUnauthorized())
                    _logger.Write(LogLevel.Error, $"{EventCodes.UnauthorizedCallToBankApi} - {ex.Message}");

                throw;
            }
        }
    }
}