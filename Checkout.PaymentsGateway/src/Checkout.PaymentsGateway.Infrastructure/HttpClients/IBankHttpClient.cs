using System.Threading.Tasks;
using Checkout.PaymentsGateway.BankIntegration.Contracts.V1.Requests;
using Checkout.PaymentsGateway.BankIntegration.Contracts.V1.Responses;

namespace Checkout.PaymentsGateway.Infrastructure.HttpClients
{
    public interface IBankHttpClient
    {
        Task<CreateBankTransactionResponse> CreateTransactionAsync(CreateBankTransactionRequest request);
    }
}