using Microsoft.Extensions.DependencyInjection;
using Sia.Core.Configuration;
using Sia.Core.Configuration.ApplicationInsights;
using Sia.Core.Configuration.Protocol;
using Sia.Core.Configuration.Sources.GitHub;

namespace Sia.Gateway.Initialization.Configuration
{
    public class GatewayConfiguration : IInjectableConfig
    {
        /// <summary>
        /// The client Id of the Gateway's AAD instance.
        /// Should be set in user secrets or environment variables.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The client secret of the Gateway's AAD instance.
        /// Should be set in user secrets or environment variables.
        /// </summary>
        public string ClientSecret { get; set; }
        /// <summary>
        /// Configuration for the KeyVault instance used to retrieve application insights config values
        /// Should be set in user secrets or environment variables.
        /// </summary>
        public ApplicationInsightsKeyVaultConfig KeyVault { get; set; }
        /// <summary>
        /// Application Insights configuration.
        /// Usually generated automatically by ApplicationInsightsStartup.
        /// </summary>
        public ApplicationInsightsConfig ApplicationInsights { get; set; }
        /// <summary>
        /// Configuration of Cross-Origin Resource Sharing.
        /// Should be set in user secrets or environment variables.
        /// </summary>
        public CorsConfig Cors { get; set; }
        /// <summary>
        /// AAD config related to the Playbook microservice
        /// Should be set in user secrets or environment variables.
        /// </summary>
        public AadRemoteResourceConfig Playbook { get; set; }
        /// <summary>
        /// AAD config related to EventUI.
        /// Should be set in user secrets or environment variables.
        /// </summary>
        public AadRemoteResourceConfig FrontEnd { get; set; }
        /// <summary>
        /// AAD config related to the login authority to use.
        /// Should be set in user secrets or environment variables.
        /// </summary>
        public AadAuthorityConfig AzureAd { get; set; }
        /// <summary>
        /// Configuration related to connectors.
        /// Should be set in user secrets or environment variables.
        /// </summary>
        public GatewayConnectorsConfig Connector { get; set; }
        /// <summary>
        /// Configuration related to communication with specific microservices.
        /// Should be set in user secrets or environment variables.
        /// </summary>
        public MicroservicesConfig Services { get; set; }
        /// <summary>
        /// Configuration related to the Redis instance used by SignalR.
        /// Should be set in user secrets or environment variables.
        /// </summary>
        public RedisConfig Redis { get; set; }
        /// <summary>
        /// Configuration for accessing secretes stored in Azure KeyVault.
        /// Should be set in appsettings.{environment}.json
        /// </summary>
        public KeyVaultAccessorConfiguration KeyVaultAccessor { get; set; }
        /// <summary>
        /// Gateway database connection string
        /// Should be set in either environement variables or from secret value in Azure KeyVault
        /// </summary>
        public string GatewayDatabaseConnectionString { get; set; }
        /// <summary>
        /// <see cref="GitHubConfiguration"/>
        /// </summary>
        public GitHubConfiguration GitHub { get; set; }

        public IServiceCollection RegisterMe(IServiceCollection services)
            => services
                .AddSingleton(this)
                .RegisterConfig(Services);
    }
}
