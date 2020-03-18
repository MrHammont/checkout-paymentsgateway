using System;
using System.Threading.Tasks;

namespace Checkout.PaymentsGateway.Infrastructure.Instrumentors
{
    public interface IInstrumentor
    {
        Task Instrument(Func<Task> aspect, string instrumentorName);
    }
}