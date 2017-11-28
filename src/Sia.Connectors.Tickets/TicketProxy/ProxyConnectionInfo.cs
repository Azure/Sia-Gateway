using Microsoft.Extensions.Configuration;
using Sia.Shared.Authentication;
using Sia.Shared.Validation;
using System;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyConnectionInfo
    {
        private AzureSecretVault _keyVault;

        /// <summary>
        /// Instantiates ProxyConnectionInfo with no Authentication
        /// </summary>
        /// <param name="endpoint">Proxy Endpoint</param>
        public ProxyConnectionInfo(string endpoint)
            : this(endpoint, AuthenticationType.None)
        { }

        /// <summary>
        /// Instantiates ProxyConnectionInfo with certificate authentication from a local cert
        /// </summary>
        /// <param name="endpoint">Proxy Endpoint</param>
        /// <param name="certThumbprint">Thumbprint for searching local certificate store</param>
        public ProxyConnectionInfo(string endpoint, string certThumbprint)
            : this(endpoint, AuthenticationType.Certificate)
        {
            CertIdentifier = ThrowIf.NullOrWhiteSpace(certThumbprint, nameof(certThumbprint));
        }

        /// <summary>
        /// Instantiates ProxyConnectionInfo with certificate authentication using a certificate retrieved from keyvault
        /// </summary>
        /// <param name="endpoint">Proxy Endpoint</param>
        /// <param name="config">Configuration root for initialization</param>
        /// <param name="vaultName">Key vault name</param>
        public ProxyConnectionInfo(string endpoint, KeyVaultConfiguration config, string vaultName)
            : this(endpoint, AuthenticationType.CertificateFromKeyVault)
        {
            _keyVault = new AzureSecretVault(config);
            CertIdentifier = ThrowIf.NullOrWhiteSpace(vaultName, nameof(vaultName));
        }

        protected ProxyConnectionInfo(string endpoint, AuthenticationType authType)
        {
            Endpoint = ThrowIf.NullOrWhiteSpace(endpoint, nameof(endpoint));
            AuthenticationType = authType;
        }

        public ProxyClient Client
        {
            get
            {
                if(_client == null)
                {
                    _client = new ProxyClient(ClientFactory.GetClient(), Endpoint);
                }
                return _client;
            }
        }
        protected ProxyClient _client;
        public AuthenticationType AuthenticationType { get; protected set; }
        public string Endpoint { get; protected set; }
        public string CertIdentifier { get; protected set; }

        protected IHttpClientFactory ClientFactory
        {
            get
            {
                switch (AuthenticationType)
                {
                    case AuthenticationType.Certificate:
                        return new LocalCertificateRetriever(CertIdentifier);
                    case AuthenticationType.CertificateFromKeyVault:
                        return new KeyVaultCertificateRetriever(_keyVault, CertIdentifier);
                    case AuthenticationType.None:
                        return new UnauthenticatedClientFactory();
                    default:
                        throw new NotImplementedException($"Unrecognized authentication type {AuthenticationType.GetName(typeof(AuthenticationType), AuthenticationType)}");
                }
            }
        }
    }
}
