using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                .AddScoped(serv => proxyConnection)
                .AddScoped<TicketingClient, ProxyClient>()
                .AddScoped<Connector, ProxyConnector>();

        public static void AddProxyConnector(
            this IServiceCollection services,
            ProxyConfig config)
        {
            switch (config.AuthType)
            {
                case ProxyConfig.CertificateAuthType:
                    services.AddProxyWithCert(
                        config.Endpoint,
                        config.Certificate.Thumbprint
                    );
                    return;
                case ProxyConfig.VaultCertificateAuthType:
                    services.AddProxyWithCertFromKeyVault(
                        config.Endpoint,
                        new KeyVaultConfiguration(
                            config.VaultCertificate.ClientId,
                            config.VaultCertificate.ClientSecret,
                            config.VaultCertificate.VaultName
                        ),
                        config.VaultCertificate.CertName
                    );
                    return;
                default:
                    services.AddProxyWithoutAuth(config.Endpoint);
                    return;
            }
        }
    }
}
