using Microsoft.Extensions.DependencyInjection;
using Sia.Connectors.Tickets;
using Sia.Connectors.Tickets.TicketProxy;

namespace Sia.Gateway.Initialization
{
    public static partial class Initialization
    {
        public static IServiceCollection AddProxyWithoutAuth(this IServiceCollection services, string endpoint)
            => services.AddProxy(new ProxyConnectionInfo(endpoint));
        

        public static IServiceCollection AddProxyWithCert(this IServiceCollection services, string endpoint, string certThumbprint)
            => services.AddProxy(new ProxyConnectionInfo(endpoint, certThumbprint));
        

        private static IServiceCollection AddProxy(this IServiceCollection services, ProxyConnectionInfo proxyConnection)
        {
            return services
                .AddScoped<Converter<Ticket>, ProxyConverter>()
                .AddScoped<Client<Ticket>>(serv => proxyConnection.GetClient())
                .AddScoped<Connector<Ticket>, ProxyConnector>();
        }
    }
}
