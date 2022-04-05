namespace Xas.LightTeams.Http
{
    internal class RefreshRequest
    {
        public string GrantType => "refresh_token";
        public string ClientId { get; set; }
        public string Scope => "user.read presence.read openid profile offline_access";
        public string RefreshToken { get; set; }

        public Dictionary<string, string> FormSerialize()
        {
            Dictionary<string, string> formSerialize = new Dictionary<string, string>();
            formSerialize.Add("grant_type", GrantType);
            formSerialize.Add("client_id", ClientId);
            formSerialize.Add("scope", Scope);
            formSerialize.Add("refresh_token", RefreshToken);
            return formSerialize;
        }
    }
}
