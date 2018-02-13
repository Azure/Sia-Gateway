using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Sia.Gateway.Initialization.Configuration;
using Sia.Shared.Authentication;
using Sia.Shared.Validation;
using System.Threading.Tasks;

namespace Sia.Gateway.Initialization
{
    public static class ApplicationInsightsStartup
    {
        public static async Task InitializeApplicationInsights(this IHostingEnvironment env, GatewayConfiguration configuration)
        {
            //Needs to be done in the initial Startup.Startup() method because Application Insights registers itself prior
            //to ConfigureServices being run
            var instrumentationKeyName = ThrowIf.NullOrWhiteSpace(configuration.KeyVault.InstrumentationKeyName, nameof(configuration.KeyVault.InstrumentationKeyName));

            if (configuration.ApplicationInsights == null)
            {
                configuration.ApplicationInsights = new Shared.Configuration.ApplicationInsights.ApplicationInsightsConfig();
            }

            var secrets = new AzureSecretVault(
                new KeyVaultConfiguration(
                    configuration.ClientId,
                    configuration.ClientSecret,
                    configuration.KeyVault.VaultName
                )
            );

            configuration.ApplicationInsights.InstrumentationKey = await secrets.Get(instrumentationKeyName).ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
