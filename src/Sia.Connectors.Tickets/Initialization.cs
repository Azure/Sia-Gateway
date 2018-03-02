using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sia.Connectors.Tickets;
using Sia.Shared.Validation;
using System;

namespace Sia.Gateway.Initialization
{
    public static partial class Initialization
    {
        public static IServiceCollection AddTicketingConnector(
            this IServiceCollection services,
            IHostingEnvironment env,
            IConfigurationRoot config,
            TicketingConnectorConfig connectorConfig)
        {
            if (connectorConfig == null)
            {
                return services.AddNoTicketingSystem();
            }

            if (!String.IsNullOrWhiteSpace(connectorConfig.Path))
            {
                return services
                    .LoadConnectorFromAssembly(
                        env,
                        config,
                        connectorConfig.Path
                    );
            }

            if (connectorConfig.Proxy != null)
            {
                return services.AddProxyConnector(connectorConfig.Proxy);
            }

            return services.AddNoTicketingSystem();
        }
    }
}
