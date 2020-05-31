using System;
using Microsoft.Extensions.Logging;

namespace MarsRover.Service
{
    internal class ServiceLogger : ILogger
    {
        private readonly Settings _settings;
        private readonly ILogger _logger;

        public ServiceLogger(Settings settings, ILoggerFactory loggerFactory)
        {
            _settings = settings;
            _logger = loggerFactory.CreateLogger<ServiceLogger>();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // Abusing a bit the Logger to be able to show internal messages in console window, according to settings
            if (logLevel == LogLevel.Debug)
            {
                if (!_settings.ShowDebugLogs)
                    return;

                _logger.Log(LogLevel.Information, eventId, state, exception, formatter);
                return;
            }

            _logger.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _logger.BeginScope(state);
        }
    }
}
