using System.Drawing;
using Xas.LightTeams.Http;

namespace Xas.LightTeams.App
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly HttpClient _httpClient;
        private readonly LightManager _lightManager;
        private readonly OAuthManager _oauthManager;
        private bool _isReady;
        private readonly Configuration _configuration = new Configuration();

        public Worker(ILogger<Worker> logger, IConfiguration config, OAuthManager oAuthManager)
        {
            _logger = logger;
            _oauthManager = oAuthManager;
            _lightManager = new LightManager();
            _httpClient = new HttpClient();
            _configuration.ClientId = config.GetValue<string>("ClientId");
            _configuration.TenantId = config.GetValue<string>("TenantId");
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Step 1 - Request Authorization
                string message = await _oauthManager.SendAuthorizationRequestAsync();

                _isReady = true;
                _logger.LogDebug("Authorization request successfull");
                _logger.LogDebug(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to initialize at the 'StartAsync'");
                return;
            }

            await base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _lightManager?.Stop();
            _httpClient?.Dispose();
            _lightManager.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_isReady)
            {
                _logger.LogWarning("'StartAsync' was not ready");
                return;
            }

            if (!await _oauthManager.AuthenticateAsync(stoppingToken))
            {
                _logger.LogWarning("Unable to authenticate");
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                if (await _oauthManager.RefreshTokenAsync())
                {
                    PresenceResponse presenceResponse = await _oauthManager.GetPresenceAsync();
                    if (PresenceDefinitions.PresenceGreen.Contains(presenceResponse.Availability))
                    {
                        _lightManager.SetStatus(Color.Green);
                    }
                    else if (PresenceDefinitions.PresenceRed.Contains(presenceResponse.Availability))
                    {
                        _lightManager.SetStatus(Color.Red);
                    }
                    else
                    {
                        _lightManager.SetStatus(Color.Blue);
                    }
                }
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
