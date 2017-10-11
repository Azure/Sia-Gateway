using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Sia.Shared.Authentication;

namespace Sia.Gateway.Initialization
{
    public static class ApplicationInsightsStartup
    {
        public static AzureSecretVault InitializeApplicationInsights(this IHostingEnvironment env, IConfigurationRoot configuration)
        {
            //Needs to be done in the initial Startup.Startup() method because Application Insights registers itself prior
            //to ConfigureServices being run
            var secrets = new AzureSecretVault(
                new KeyVaultConfiguration(
                    configuration["ClientId"],
                    configuration["ClientSecret"],
                    configuration["KeyVault:VaultName"]
                )
            );

            var instrumentationKey = configuration.GetSection("KeyVault")["InstrumentationKeyName"];
            if (!string.IsNullOrWhiteSpace(instrumentationKey))
            {
                var vaultTask = secrets.Get(instrumentationKey);
                vaultTask.Wait();
                configuration.GetSection("ApplicationInsights")["InstrumentationKey"] = vaultTask.Result;
            }

            return secrets;
        }
    }
}
