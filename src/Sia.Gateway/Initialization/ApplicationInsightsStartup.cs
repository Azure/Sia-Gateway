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

            var vaultTask = secrets.Get(configuration.GetSection("KeyVault")["InstrumentationKeyName"]);
            vaultTask.Wait();
            configuration.GetSection("ApplicationInsights")["InstrumentationKey"] = vaultTask.Result;

            return secrets;
        }
    }
}
