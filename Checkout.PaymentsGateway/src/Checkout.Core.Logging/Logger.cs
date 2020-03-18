using System;
using Microsoft.Extensions.Logging;

namespace Checkout.Core.Logging
{
    public class Logger : IAppLogger
    {
        private readonly ILogger<Logger> _internalLogger;

        public Logger(ILogger<Logger> internalLogger)
        {
            _internalLogger = internalLogger;
        }

        public Guid CorrelationId { get; set; }

        public void Write(LogLevel level, string message)
        {
            _internalLogger.Log(level, message);
        }
    }
}