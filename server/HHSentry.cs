using Sentry;

namespace HelteOgHulerServer;

public static class HHSentry
{
    public static void Initialize(IConfigurationSection sentryConfig)
    {
        if (String.IsNullOrWhiteSpace(sentryConfig["DSN"]))
        {
            return;
        }

        SentrySdk.Init(options =>
        {
            options.Dsn = sentryConfig["DSN"];

            options.AutoSessionTracking = true;

            options.TracesSampleRate = 0.01;

            options.ProfilesSampleRate = 0.01;
        });
    }
}