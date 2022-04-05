using System.Text.Json.Serialization;

namespace NLog.Telegram
{
    public class MessageRequest
    {
        [JsonPropertyName("chat_id")]
        public string ChatId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("disable_web_page_preview")]
        public string DisableWebPagePreview { get; set; }

        [JsonPropertyName("reply_to_message_id")]
        public int? ReplyToMessageId { get; set; }

        [JsonPropertyName("parse_mode")]
        public string ParseMode { get; set; }
    }
}