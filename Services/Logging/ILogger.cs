using CMZero.Web.Models.Logging;

namespace CMZero.Web.Services.Logging
{
    public interface ILogger
    {
        void LogEvent(LogEvent logEvent);
    }
}