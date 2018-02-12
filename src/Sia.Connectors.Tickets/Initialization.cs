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
        public static void AddTicketingConnector(
            this IServiceCollection services,
            IHostingEnvironment env,
            IConfigurationRoot config,
            TicketingConnectorConfig connectorConfig)
        {
            if (!String.IsNullOrWhiteSpace(connectorConfig.Path))
            {
                services
                    .LoadConnectorFromAssembly(
                        env,
                        config,
                        connectorConfig.Path
                    );
                return;
            }

            if (connectorConfig.Proxy != null)
            {
                services.AddProxyConnector(connectorConfig.Proxy);
                return;
            }

            services.AddNoTicketingSystem();
        }
    }
}
