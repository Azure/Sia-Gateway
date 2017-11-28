using Microsoft.Extensions.DependencyInjection;
using Sia.Connectors.Tickets;
using Sia.Connectors.Tickets.None;

namespace Sia.Gateway.Initialization
{
    public static partial class Initialization
    {
        public static IServiceCollection AddNoTicketingSystem(
            this IServiceCollection services
        ) => services
                .AddSingleton<NoClient>()
                .AddSingleton<Connector, NoConnector>();
    }
}
