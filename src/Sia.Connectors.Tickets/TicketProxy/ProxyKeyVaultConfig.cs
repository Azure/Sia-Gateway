using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyKeyVaultConfig
    {
        /// <summary>
        /// ClientId to use when authenticating to KeyVault
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Client Secret to use when authenticating to KeyVault
        /// </summary>
        public string ClientSecret { get; set; }
        /// <summary>
        /// Name of Key Vault instance to connect to when retrieving certificate to use.
        /// </summary>
        public string VaultName { get; set; }
        /// <summary>
        /// Name of Certificate within Key Vault.
        /// </summary>
        public string CertName { get; set; }
    }
}
