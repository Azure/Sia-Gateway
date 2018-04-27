using Sia.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyKeyVaultConfig : KeyVaultConfiguration
    {
        /// <summary>
        /// Name of Certificate within Key Vault.
        /// </summary>
        public string CertName { get; set; }
    }
}
