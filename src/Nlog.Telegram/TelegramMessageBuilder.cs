using System;
using System.Threading.Tasks;

namespace NLog.Telegram
{
    public class TelegramMessageBuilder : IDisposable
    {
        private readonly string _baseUrl;

        private readonly TelegramClient _client;

        private readonly MessageRequest _request;

        /// <summary>
        /// telegram text restriction
        /// </summary>
        private readonly static int MaxTextLength = 4096;

        public TelegramMessageBuilder(string baseUrl, string botToken)
        {
            _baseUrl = $"{baseUrl}{botToken}/sendMessage";
            _client = new TelegramClient();
            _request = new MessageRequest() { ParseMode = "MARKDOWN" };
        }

        public TelegramMessageBuilder OnError(Action<Exception> error)
        {
            _client.Error += error;

            return this;
        }

        public TelegramMessageBuilder ToChat(string chatId)
        {
            _request.ChatId = chatId;
            return this;
        }

        public TelegramMessageBuilder ToFormat(string format)
        {
            _request.ParseMode = format;
            return this;
        }

        public async Task SendAsync(string message)
        {
            _request.Text = message?.Substring(0, Math.Min(MaxTextLength, message.Length));

            await _client.SendAsync(_baseUrl, _request);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}