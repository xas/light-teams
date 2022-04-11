using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xas.LightTeams.Http;

namespace Xas.LightTeams.App
{
    public class OAuthManager : IDisposable
    {
        private readonly ILogger<OAuthManager> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _tenantId;
        private readonly string _clientId;
        private string _deviceCode;
        private string _refreshToken;
        private string _accessToken;
        private bool _isAuthenticated;
        private const int _delayInterval = -10000 * 2;

        public DateTime DelayToAuthenticate { get; private set; }
        public DateTime DelayBeforeRefresh { get; private set; }

        public OAuthManager(ILogger<OAuthManager> logger, IConfiguration config)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _tenantId = config.GetValue<string>("TenantId");
            _clientId = config.GetValue<string>("ClientId");
        }

        public async Task<string> SendAuthorizationRequestAsync()
        {
            AuthorizationRequest authorizationRequest = new AuthorizationRequest() { ClientId = _clientId };
            HttpResponseMessage response = await _httpClient.PostAsync($"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/devicecode", new FormUrlEncodedContent(authorizationRequest.FormSerialize()));
            if (!response.IsSuccessStatusCode)
            {
                string error = $"An error occured when getting authentification device code : {response.ReasonPhrase} - {await response.Content.ReadAsStringAsync()}";
                _logger.LogError(error);
                throw new HttpRequestException(error, null, response.StatusCode);
            }

            AuthorizationDeviceResponse deviceResponse = await response.Content.ReadFromJsonAsync<AuthorizationDeviceResponse>();
            DelayToAuthenticate = DateTime.UtcNow.AddSeconds(deviceResponse.Expires);
            _deviceCode = deviceResponse.DeviceCode;
            _logger.LogInformation(deviceResponse.Message);
            return deviceResponse.Message;
        }

        public async Task<bool> AuthenticateAsync(CancellationToken cancellationToken)
        {
            AuthentificationRequest authentificationRequest = new AuthentificationRequest() { ClientId = _clientId, DeviceCode = _deviceCode };
            FormUrlEncodedContent content = new FormUrlEncodedContent(authentificationRequest.FormSerialize());
            HttpResponseMessage response;

            while (!cancellationToken.IsCancellationRequested)
            {
                if (DateTime.UtcNow > DelayToAuthenticate)
                {
                    _logger.LogError($"Unable to authenticate before expiration time {nameof(DelayToAuthenticate)}");
                    throw new ArgumentOutOfRangeException(nameof(DelayToAuthenticate), "Unable to authenticate before expiration time");
                }
                response = await _httpClient.PostAsync($"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/token", content);
                if (response.IsSuccessStatusCode)
                {
                    AuthentificationResponse authentificationResponse = await response.Content.ReadFromJsonAsync<AuthentificationResponse>();
                    DelayBeforeRefresh = DateTime.UtcNow.AddSeconds(authentificationResponse.Expires).AddMilliseconds(_delayInterval);
                    _refreshToken = authentificationResponse.RefreshToken;
                    _accessToken = authentificationResponse.AccessToken;
                    _isAuthenticated = true;
                    return true;
                }
                await Task.Delay(5000, cancellationToken);
            }

            return false;
        }

        public async Task<bool> RefreshTokenAsync()
        {
            if (DateTime.UtcNow > DelayBeforeRefresh)
            {
                // Refresh the access token
                RefreshRequest refreshRequest = new RefreshRequest() { ClientId = _clientId, RefreshToken = _refreshToken };
                FormUrlEncodedContent content = new FormUrlEncodedContent(refreshRequest.FormSerialize());
                HttpResponseMessage response = await _httpClient.PostAsync($"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/token", content);
                if (response.IsSuccessStatusCode)
                {
                    AuthentificationResponse authentificationResponse = await response.Content.ReadFromJsonAsync<AuthentificationResponse>();
                    DelayBeforeRefresh = DateTime.UtcNow.AddSeconds(authentificationResponse.Expires).AddMilliseconds(_delayInterval);
                    _refreshToken = authentificationResponse.RefreshToken;
                    _accessToken = authentificationResponse.AccessToken;
                    _isAuthenticated = true;
                }
                else
                {
                    _logger.LogError($"Unable to refresh token {response.ReasonPhrase} - {await response.Content.ReadAsStringAsync()}");
                    _isAuthenticated = false;
                    return false;
                }
            }
            return true;
        }

        public async Task<PresenceResponse> GetPresenceAsync()
        {
            if (!_isAuthenticated)
            {
                throw new TimeoutException("User is not authenticated");
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            HttpResponseMessage response = await _httpClient.GetAsync("https://graph.microsoft.com/beta/me/presence");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PresenceResponse>();
            }
            string error = $"An error occured when getting presence status : {response.ReasonPhrase} - {await response.Content.ReadAsStringAsync()}";
            _logger.LogError(error);
            throw new HttpRequestException(error, null, response.StatusCode);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
