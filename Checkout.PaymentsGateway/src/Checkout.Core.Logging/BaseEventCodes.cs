using System;
using System.Diagnostics.Tracing;

namespace Checkout.Core.Logging
{
    public static class BaseEventCodes
    {
        public static string EventCode(EventLevel level, string eventName, string prefix)
        {
            return $"{GetEventLevelCode(level)}_CHECKOUT_{prefix}_{eventName}";
        }

        private static string GetEventLevelCode(EventLevel level)
        {
            switch (level)
            {
                case EventLevel.Critical: return "CRITICAL";
                case EventLevel.Error: return "ERROR";
                case EventLevel.Warning: return "WARN";
                case EventLevel.Informational: return "INFO";
                default: throw new InvalidCastException();
            }
        }
    }
}