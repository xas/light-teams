using NLog.Common;
using NLog.Config;
using NLog.Targets;
using System;
using System.Threading.Tasks;

namespace NLog.Telegram
{
    [Target("Telegram")]
    public class TelegramTarget : TargetWithLayout
    {
        public TelegramTarget()
        {
            BaseUrl = "https://api.telegram.org/bot";
        }

        public string BaseUrl { get; set; }

        [RequiredParameter]
        public string BotToken { get; set; }

        [RequiredParameter]
        public string ChatId { get; set; }

        public string Format { get; set; }

        private TelegramMessageBuilder _builder;

        protected override void InitializeTarget()
        {
            if (string.IsNullOrWhiteSpace(BotToken))
                throw new ArgumentOutOfRangeException("BotToken", "BotToken cannot be empty.");

            if (string.IsNullOrWhiteSpace(ChatId))
                throw new ArgumentOutOfRangeException("ChatId", "ChatId cannot be empty.");

            _builder = new TelegramMessageBuilder(BaseUrl, BotToken);
            _builder.ToChat(ChatId);
            _builder.ToFormat(Format ?? "MARKDOWN");

            base.InitializeTarget();
        }

        protected override async void Write(AsyncLogEventInfo info)
        {
            try
            {
                await SendAsync(info);
            }
            catch (Exception e)
            {
                info.Continuation(e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _builder?.Dispose();
            base.Dispose(disposing);
        }

        private async Task SendAsync(AsyncLogEventInfo info)
        {
            var message = Layout.Render(info.LogEvent);
            await _builder.SendAsync(message);
        }
    }
}
