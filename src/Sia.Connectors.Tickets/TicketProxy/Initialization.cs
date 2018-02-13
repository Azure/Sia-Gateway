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
        public static IServiceCollection AddProxyConnector(
            this IServiceCollection services,
            ProxyConfig config)
        {
            ProxyConnectionInfo connectionInfo;
            switch (config.AuthType)
            {
                case ProxyConfig.CertificateAuthType:
                    connectionInfo = new ProxyConnectionInfo(config.Endpoint, config.Certificate.Thumbprint);
                    break;
                case ProxyConfig.VaultCertificateAuthType:
                    connectionInfo = new ProxyConnectionInfo(
                        config.Endpoint,
                        new KeyVaultConfiguration(
                            config.VaultCertificate.ClientId,
                            config.VaultCertificate.ClientSecret,
                            config.VaultCertificate.VaultName
                        ), 
                        config.VaultCertificate.CertName
                    );
                    break;
                default:
                    connectionInfo = new ProxyConnectionInfo(config.Endpoint);
                    break;
            }
            return services
                .AddScoped(serv => connectionInfo)
                .AddScoped<TicketingClient, ProxyClient>()
                .AddScoped<Connector, ProxyConnector>();
        }
    }
}
