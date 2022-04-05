namespace Xas.LightTeams.Http
{
    internal class AuthentificationRequest
    {
        public string GrantType => "urn:ietf:params:oauth:grant-type:device_code";
        public string ClientId { get; set; }
        public string DeviceCode { get; set; }

        public Dictionary<string, string> FormSerialize()
        {
            Dictionary<string, string> formSerialize = new Dictionary<string, string>();
            formSerialize.Add("grant_type", GrantType);
            formSerialize.Add("client_id", ClientId);
            formSerialize.Add("device_code", DeviceCode);
            return formSerialize;
        }
    }
}
