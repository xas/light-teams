using System.Text.Json.Serialization;

namespace Xas.LightTeams.Http
{
    public class PresenceResponse
    {
        [JsonPropertyName("@odata.context")]
        public string Context { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("availability")]
        public string Availability { get; set; }
        [JsonPropertyName("activity")]
        public string Activity { get; set; }
        [JsonPropertyName("outOfOfficeSettings")]
        public OutOfOfficeSettings OutOfOffice { get; set; }
    }

    public class OutOfOfficeSettings
    {
        [JsonPropertyName("message")]
        public string Activity { get; set; }
        [JsonPropertyName("isOutOfOffice")]
        public bool IsOutOfOffice { get; set; }
    }
}
