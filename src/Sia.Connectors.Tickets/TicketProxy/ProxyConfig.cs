using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyConfig
    {
        /// <summary>
        /// Proxy endpoint to use
        /// </summary>
        public string Endpoint { get; set; }
        /// <summary>
        /// Determines which auth type to use for communication with the proxy API.
        /// </summary>
        public string AuthType { get; set; }
        /// <summary>
        /// Configuration related to the 'Certificate' authType
        /// </summary>
        public ProxyLocalCertificateConfig Certificate { get; set; }
        /// <summary>
        /// Configuration related to the 'VaultCertificate' authType
        /// </summary>
        public ProxyKeyVaultConfig VaultCertificate { get; set; }

        public const string CertificateAuthType = "Certificate";
        public const string VaultCertificateAuthType = "VaultCertificate";
    }
}
