namespace Xas.LightTeams.Http
{
    internal class AuthorizationRequest
    {
        public string ClientId { get; set; }
        public string Scope => "user.read presence.read openid profile offline_access";

        public Dictionary<string, string> FormSerialize()
        {
            Dictionary<string, string> formSerialize = new Dictionary<string, string>();
            formSerialize.Add("client_id", ClientId);
            formSerialize.Add("scope", Scope);
            return formSerialize;
        }
    }
}
