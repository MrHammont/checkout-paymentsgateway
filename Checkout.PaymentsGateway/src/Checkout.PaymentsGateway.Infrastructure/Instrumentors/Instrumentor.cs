using System;
using System.Threading.Tasks;
using Checkout.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentsGateway.Infrastructure.Instrumentors
{
    public class Instrumentor : IInstrumentor
    {
        private readonly IAppLogger _logger;

        public Instrumentor(IAppLogger logger)
        {
            _logger = logger;
        }

        public async Task Instrument(Func<Task> aspect, string instrumentorName)
        {
            var instrumentation = new Instrumentation(instrumentorName);

            instrumentation.Stopwatch.Start();

            await aspect();

            instrumentation.Stopwatch.Stop();

            _logger.Write(LogLevel.Information,
                $"{EventCodes.EventInstrumented(instrumentation.Name)} - Time taken: {instrumentation.Stopwatch.Elapsed}");
        }
    }
}