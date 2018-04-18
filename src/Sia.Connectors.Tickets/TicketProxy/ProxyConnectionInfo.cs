using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sia.Core.Authentication;
using Sia.Core.Validation;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyConnectionInfo
    {
        

        /// <summary>
        /// Instantiates ProxyConnectionInfo with no Authentication
        /// </summary>
        /// <param name="endpoint">Proxy Endpoint</param>
        public ProxyConnectionInfo(
            string endpoint
        ) : this(
            endpoint, 
            AuthenticationType.None)
        { }

        /// <summary>
        /// Instantiates ProxyConnectionInfo with certificate authentication from a local cert
        /// </summary>
        /// <param name="endpoint">Proxy Endpoint</param>
        /// <param name="certThumbprint">Thumbprint for searching local certificate store</param>
        public ProxyConnectionInfo(
            string endpoint, 
            string certThumbprint
        ) : this(
            endpoint,
            AuthenticationType.Certificate
        )
        {
            CertIdentifier = ThrowIf.NullOrWhiteSpace(certThumbprint, nameof(certThumbprint));
        }

        /// <summary>
        /// Instantiates ProxyConnectionInfo with certificate authentication using a certificate retrieved from keyvault
        /// </summary>
        /// <param name="endpoint">Proxy Endpoint</param>
        /// <param name="config">Configuration root for initialization</param>
        /// <param name="vaultName">Key vault name</param>
        public ProxyConnectionInfo(
            string endpoint,
            KeyVaultConfiguration config, 
            string vaultName
        ) : this(
            endpoint,
            AuthenticationType.CertificateFromKeyVault
        )
        {
            Vault = new AzureSecretVault(config);
            CertIdentifier = ThrowIf.NullOrWhiteSpace(vaultName, nameof(vaultName));
        }

        protected ProxyConnectionInfo(
            string endpoint,
            AuthenticationType authType
        )
        {
            Endpoint = ThrowIf.NullOrWhiteSpace(endpoint, nameof(endpoint));
            AuthenticationType = authType;
        }

        public async Task<HttpClient> GetClientAsync (ILoggerFactory loggerFactory)
        {
            if(_client is null)
            {
                _client = await ClientFactory(loggerFactory).GetClientAsync().ConfigureAwait(continueOnCapturedContext: false);
            }
            return _client;
        }

        private HttpClient _client;
        private readonly AzureSecretVault Vault;
        public AuthenticationType AuthenticationType { get; protected set; }
        public string Endpoint { get; }
        private readonly string CertIdentifier;

        protected IHttpClientFactory ClientFactory(ILoggerFactory loggerFactory)
        {
            switch (AuthenticationType)
            {
                case AuthenticationType.Certificate:
                    return new LocalCertificateRetriever(CertIdentifier, loggerFactory);
                case AuthenticationType.CertificateFromKeyVault:
                    return new KeyVaultCertificateRetriever(Vault, CertIdentifier, loggerFactory);
                case AuthenticationType.None:
                    return new UnauthenticatedClientFactory();
                default:
                    throw new NotImplementedException($"Unrecognized authentication type {Enum.GetName(typeof(AuthenticationType), AuthenticationType)}");
            }
        }
    }
}
