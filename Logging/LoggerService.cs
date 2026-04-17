using System;
using Serilog;

namespace PharmacyAPI.Logging
{
    public class LoggerService
    {
        public void LogError(string message, Exception ex)
        {
            Log.Error(ex, message);
        }

        public void LogInfo(string message)
        {
            Log.Information(message);
        }

        public void LogWarning(string message)
        {
            Log.Warning(message);
        }
    }
}