using Checkout.PaymentsGateway.Domain.Models;
using System.Threading.Tasks;

namespace Checkout.PaymentsGateway.Domain.Repositories
{
    public interface IBankRepository
    {
        Task<TransactionResult> CreateTransactionAsync(BankTransaction transaction);
    }
}