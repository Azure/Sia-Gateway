using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Sia.Gateway.Initialization.Configuration;
using Sia.Shared.Authentication;
using Sia.Shared.Validation;

namespace Sia.Gateway.Initialization
{
    public static class ApplicationInsightsStartup
    {
        public static void InitializeApplicationInsights(this IHostingEnvironment env, GatewayConfiguration configuration)
        {
            //Needs to be done in the initial Startup.Startup() method because Application Insights registers itself prior
            //to ConfigureServices being run

            var secrets = new AzureSecretVault(
                new KeyVaultConfiguration(
                    configuration.ClientId,
                    configuration.ClientSecret,
                    configuration.KeyVault.VaultName
                )
            );

            var instrumentationKeyName = ThrowIf.NullOrWhiteSpace(configuration.KeyVault.InstrumentationKeyName, nameof(configuration.KeyVault.InstrumentationKeyName));

            var vaultTask = secrets.Get(instrumentationKeyName);
            vaultTask.Wait();

            if(configuration.ApplicationInsights == null)
            {
                configuration.ApplicationInsights = new Shared.Configuration.ApplicationInsights.ApplicationInsightsConfig();
            }
            configuration.ApplicationInsights.InstrumentationKey = vaultTask.Result;
        }
    }
}
