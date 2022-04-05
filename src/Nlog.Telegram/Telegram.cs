using NLog;
using NLog.Config;
using NLog.Targets;

namespace Nlog.Telegram
{
    [Target("Telegram")]
    public sealed class TelegramTarget : TargetWithLayout
    {
        public TelegramTarget()
        {
        }

        [RequiredParameter]
        public string Host { get; set; }
        protected override void Write(LogEventInfo logEvent)
        {
            string logMessage = Layout.Render(logEvent);
        }
    }
}
