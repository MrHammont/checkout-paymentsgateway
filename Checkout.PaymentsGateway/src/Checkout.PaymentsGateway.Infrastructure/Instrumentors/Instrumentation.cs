using System.Diagnostics;

namespace Checkout.PaymentsGateway.Infrastructure.Instrumentors
{
    public class Instrumentation
    {
        public Instrumentation(string name)
        {
            Name = name;
            Stopwatch = new Stopwatch();
        }

        public string Name { get; }
        public Stopwatch Stopwatch { get; }
    }
}