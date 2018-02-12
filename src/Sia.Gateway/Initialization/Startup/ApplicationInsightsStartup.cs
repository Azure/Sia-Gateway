using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Sia.Gateway.Initialization.Configuration;
using Sia.Shared.Authentication;
using Sia.Shared.Validation;

namespace Sia.Gateway.Initialization
{
    public static class ApplicationInsightsStartup
    {
        public static void InitializeApplicationInsights(this IHostingEnvironment env, IConfigurationRoot configuration)
        {
            //Needs to be done in the initial Startup.Startup() method because Application Insights registers itself prior
            //to ConfigureServices being run
            var deserializedConfig = configuration.Get<GatewayConfiguration>();

            var secrets = new AzureSecretVault(
                new KeyVaultConfiguration(
                    deserializedConfig.ClientId,
                    deserializedConfig.ClientSecret,
                    deserializedConfig.KeyVault.VaultName
                )
            );

            var instrumentationKeyName = ThrowIf.NullOrWhiteSpace(deserializedConfig.KeyVault.InstrumentationKeyName, nameof(deserializedConfig.KeyVault.InstrumentationKeyName));

            var vaultTask = secrets.Get(instrumentationKeyName);
            vaultTask.Wait();

            if(deserializedConfig.ApplicationInsights == null)
            {
                deserializedConfig.ApplicationInsights = new Shared.Configuration.ApplicationInsights.ApplicationInsightsConfig();
            }
            deserializedConfig.ApplicationInsights.InstrumentationKey = vaultTask.Result;
        }
    }
}
