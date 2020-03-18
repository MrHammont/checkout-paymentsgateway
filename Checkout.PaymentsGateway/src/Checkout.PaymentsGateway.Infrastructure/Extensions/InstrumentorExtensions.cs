using System;
using System.Threading.Tasks;
using Checkout.PaymentsGateway.Infrastructure.Instrumentors;

namespace Checkout.PaymentsGateway.Infrastructure.Extensions
{
    public static class InstrumentorExtensions
    {
        public static async Task<T> ApplyAsync<T>(
            this IInstrumentor instrumentor,
            Func<Task<T>> aspect,
            string instrumentorName)
        {
            var result = default(T);

            await instrumentor.Instrument(
                async () => result = await aspect.Invoke(),
                instrumentorName);

            return result;
        }

        public static async Task ApplyAsync(
            this IInstrumentor instrumentor,
            Func<Task> aspect,
            string instrumentorName)
        {
            await instrumentor.Instrument(
                async () => await aspect.Invoke(),
                instrumentorName);
        }
    }
}