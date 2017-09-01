using Sia.Shared.Authentication;
using Sia.Shared.Validation;
using System;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyConnectionInfo
    {
        public ProxyConnectionInfo(string endpoint)
            : this(endpoint, AuthenticationType.None)
        { }

        public ProxyConnectionInfo(string endpoint, string certThumbprint)
            : this(endpoint, AuthenticationType.Certificate)
        {
            CertThumbprint = ThrowIf.NullOrWhiteSpace(certThumbprint, nameof(certThumbprint));
        }

        protected ProxyConnectionInfo(string endpoint, AuthenticationType authType)
        {
            Endpoint = ThrowIf.NullOrWhiteSpace(endpoint, nameof(endpoint));
            AuthenticationType = authType;
        }

        public ProxyClient GetClient() => new ProxyClient(ClientFactory.GetClient(), Endpoint);
        public AuthenticationType AuthenticationType { get; set; }
        public string Endpoint { get; set; }
        public string CertThumbprint { get; set; }

        protected IHttpClientFactory ClientFactory
        {
            get
            {
                switch (AuthenticationType)
                {
                    case AuthenticationType.Certificate:
                        return new LocalCertificateRetriever(CertThumbprint);
                    case AuthenticationType.None:
                        return new UnauthenticatedClientFactory();
                    default:
                        throw new NotImplementedException($"Unrecognized authentication type {AuthenticationType.GetName(typeof(AuthenticationType), AuthenticationType)}");
                }
            }
        }
    }
}
