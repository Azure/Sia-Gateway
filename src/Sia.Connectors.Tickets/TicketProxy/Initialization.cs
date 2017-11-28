using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sia.Connectors.Tickets;
using Sia.Connectors.Tickets.TicketProxy;
using Sia.Shared.Authentication;

namespace Sia.Gateway.Initialization
{
    public static partial class Initialization
    {
        public static IServiceCollection AddProxyWithoutAuth(
            this IServiceCollection services,
            string endpoint
        ) => services.AddProxy(new ProxyConnectionInfo(endpoint));
        

        public static IServiceCollection AddProxyWithCert(
            this IServiceCollection services,
            string endpoint,
            string certThumbprint
        ) => services.AddProxy(new ProxyConnectionInfo(endpoint, certThumbprint));


        public static IServiceCollection AddProxyWithCertFromKeyVault(
            this IServiceCollection services,
            string endpoint,
            KeyVaultConfiguration config,
            string certName
        ) => services.AddProxy(new ProxyConnectionInfo(endpoint, config, certName));

        private static IServiceCollection AddProxy(
            this IServiceCollection services,
            ProxyConnectionInfo proxyConnection
        ) => services
                .AddScoped<TicketingClient>(serv => proxyConnection.Client)
                .AddScoped<Connector, ProxyConnector>();

        public static void AddProxyConnector(this IServiceCollection services, IConfigurationRoot config, string proxyEndpoint)
        {
            var proxyAuthType = config["Connector:Ticket:ProxyAuthType"];
            switch (proxyAuthType)
            {
                case "Certificate":
                    services.AddProxyWithCert(proxyEndpoint, config["Connector:Ticket:ProxyCertThumbprint"]);
                    return;
                case "VaultCertificate":
                    services.AddProxyWithCertFromKeyVault(
                        proxyEndpoint,
                        new KeyVaultConfiguration(
                            config["ClientId"],
                            config["ClientSecret"],
                            config["Connector:Ticket:VaultName"]
                        ),
                        config["Connector:Ticket:CertName"]
                    );
                    return;
                default:
                    services.AddProxyWithoutAuth(proxyEndpoint);
                    return;
            }
        }
    }
}
