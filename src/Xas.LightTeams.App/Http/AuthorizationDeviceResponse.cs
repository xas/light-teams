using System.Text.Json.Serialization;

namespace Xas.LightTeams.Http
{
    public class AuthorizationDeviceResponse
    {
        [JsonPropertyName("device_code")]
        public string DeviceCode { get; set; }
        [JsonPropertyName("user_code")]
        public string UserCode { get; set; }
        [JsonPropertyName("verification_uri")]
        public string VerificationUri { get; set; }
        [JsonPropertyName("expires_in")]
        public int Expires { get; set; }
        [JsonPropertyName("interval")]
        public int Interval { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
