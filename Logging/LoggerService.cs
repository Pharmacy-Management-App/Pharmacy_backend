using System;
using Serilog;

namespace PharmacyAPI.Logging
{
    public class LoggerService
    {
        private readonly Serilog.ILogger _logger;

        public LoggerService()
        {
            _logger = Log.Logger;
        }

        public void LogInfo(string message)
        {
            _logger.Information("[INFO] {Message} | Time: {Time}", message, DateTime.UtcNow);
        }

        public void LogWarning(string message)
        {
            _logger.Warning("[WARNING] {Message} | Time: {Time}", message, DateTime.UtcNow);
        }

        public void LogError(string message, Exception? ex = null)
        {
            if (ex != null)
                _logger.Error(ex, "[ERROR] {Message} | Time: {Time}", message, DateTime.UtcNow);
            else
                _logger.Error("[ERROR] {Message} | Time: {Time}", message, DateTime.UtcNow);
        }
    }
}
