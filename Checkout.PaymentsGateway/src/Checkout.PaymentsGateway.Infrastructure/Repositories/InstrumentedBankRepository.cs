using System.Threading.Tasks;
using Checkout.PaymentsGateway.Domain.Models;
using Checkout.PaymentsGateway.Domain.Repositories;
using Checkout.PaymentsGateway.Infrastructure.Extensions;
using Checkout.PaymentsGateway.Infrastructure.Instrumentors;

namespace Checkout.PaymentsGateway.Infrastructure.Repositories
{
    public class InstrumentedBankRepository : IBankRepository
    {
        private readonly IBankRepository _instrumentedBankRepository;
        private readonly IInstrumentor _instrumentor;

        public InstrumentedBankRepository(IBankRepository instrumentedBankRepository, IInstrumentor instrumentor)
        {
            _instrumentedBankRepository = instrumentedBankRepository;
            _instrumentor = instrumentor;
        }

        public async Task<TransactionResult> CreateTransactionAsync(BankTransaction transaction)
        {
            var result = await _instrumentor.ApplyAsync(
                () => _instrumentedBankRepository.CreateTransactionAsync(transaction),
                nameof(InstrumentedBankRepository));

            return result;
        }
    }
}