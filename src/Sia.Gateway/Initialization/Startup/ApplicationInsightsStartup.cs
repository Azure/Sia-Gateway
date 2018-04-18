using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Sia.Gateway.Initialization.Configuration;
using Sia.Core.Authentication;
using Sia.Core.Validation;
using System.Threading.Tasks;
using Sia.Core.Configuration.ApplicationInsights;

namespace Sia.Gateway.Initialization
{
    public static class ApplicationInsightsStartup
    {
        public static async Task InitializeApplicationInsights(this GatewayConfiguration configuration)
        {
            //Needs to be done in the initial Startup.Startup() method because Application Insights registers itself prior
            //to ConfigureServices being run
            var instrumentationKeyName = ThrowIf.NullOrWhiteSpace(configuration.KeyVault.InstrumentationKeyName, nameof(configuration.KeyVault.InstrumentationKeyName));

            if (configuration.ApplicationInsights == null)
            {
                configuration.ApplicationInsights = new ApplicationInsightsConfig();
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
