using System.Text;
using System.Text.Json.Serialization;

namespace Xas.LightTeams.Http
{
    public class AuthentificationResponse
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
        [JsonPropertyName("expires_in")]
        public int Expires { get; set; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        public override string ToString()
        {
            StringBuilder stb = new StringBuilder();
            stb.Append("Type : ").AppendLine(TokenType);
            stb.Append("access token : ").AppendLine(AccessToken);
            stb.Append("id token : ").AppendLine(IdToken);
            stb.Append("refresh token : ").AppendLine(RefreshToken);
            stb.Append("Expiration : ").AppendLine(Expires.ToString());
            return stb.ToString();
        }
    }
}
