using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NLog.Telegram
{
    public class TelegramClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        public event Action<Exception> Error;

        public TelegramClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task SendAsync(string url, MessageRequest request)
        {
            try
            {
                string jsonRequest = JsonSerializer.Serialize(request);
                StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync(url, content);
            }
            catch (Exception e)
            {
                OnError(e);
            }
        }

        private void OnError(Exception obj)
        {
            if (Error != null)
                Error(obj);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
