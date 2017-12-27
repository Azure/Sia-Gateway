using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sia.Shared.Validation;

namespace Sia.Gateway.Initialization
{
    public static partial class Initialization
    {
        public static void AddTicketingConnector(
            this IServiceCollection services,
            IHostingEnvironment env,
            IConfigurationRoot config)
        {
            if (TryGetConfigValue(
                    config,
                    "Connector:Ticket:Path",
                    out var ticketConnectorAssemblyPath))
            {
                services
                    .LoadConnectorFromAssembly(
                        env,
                        config,
                        ticketConnectorAssemblyPath
                    );
                return;
            }

            if (TryGetConfigValue(
                    config,
                    "Connector:Ticket:ProxyEndpoint",
                    out var proxyEndpoint))
            {
                services.AddProxyConnector(config, proxyEndpoint);
                return;
            }

            services.AddNoTicketingSystem();
        }

        private static bool TryGetConfigValue(
            this IConfigurationRoot config,
            string configName,
            out string configValue)
        {
            ThrowIf.NullOrWhiteSpace(configName, nameof(configName));
            configValue = config[configName];
            return !string.IsNullOrEmpty(configValue);
        }
    }
}
