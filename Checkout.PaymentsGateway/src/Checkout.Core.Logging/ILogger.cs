using System;
using Microsoft.Extensions.Logging;

namespace Checkout.Core.Logging
{
    public interface IAppLogger
    {
        Guid CorrelationId { get; set; }

        void Write(LogLevel level, string message);
    }
}