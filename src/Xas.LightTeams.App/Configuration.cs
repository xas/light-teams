using Xas.LightTeams.Http;

namespace Xas.LightTeams.App
{
    internal class Configuration
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public AuthorizationDeviceResponse AuthorizationResponse { get; set; }
        public AuthentificationResponse AuthentificationResponse { get; set; }
        public DateTime DelayToAuthenticate { get; set; }
        public DateTime DelayBeforeRefresh { get; set; }
    }

    internal static class PresenceDefinitions
    {
        public static List<string> PresenceGreen = new List<string> { "Available", "AvailableIdle", "Away", "BeRightBack" };
        public static List<string> PresenceRed = new List<string> { "Busy", "BusyIdle", "DoNotDisturb" };
        public static List<string> PresenceUnknown = new List<string> { "Offline", "PresenceUnknown" };
    }
}
