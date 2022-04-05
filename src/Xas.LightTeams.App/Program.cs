using NLog.Extensions.Logging;
using Xas.LightTeams.App;

IHost host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging(loggingBuilder =>
        {
            // configure Logging with NLog
            loggingBuilder.ClearProviders();
            loggingBuilder.AddNLog();
        });
        services.AddSingleton<OAuthManager>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
