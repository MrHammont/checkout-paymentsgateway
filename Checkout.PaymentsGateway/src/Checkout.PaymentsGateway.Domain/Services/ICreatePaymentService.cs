using System.Threading.Tasks;
using Checkout.PaymentsGateway.Domain.Models;

namespace Checkout.PaymentsGateway.Domain.Services
{
    public interface ICreatePaymentService
    {
        Task<TransactionResult> CreateTransaction(BankTransaction bankTransaction);
    }
}