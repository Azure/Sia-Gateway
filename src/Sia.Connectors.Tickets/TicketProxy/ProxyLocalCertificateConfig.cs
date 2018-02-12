using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyLocalCertificateConfig
    {
        /// <summary>
        /// Thumbprint of certificate to use from local store if AuthType is 'Certificate'
        /// </summary>
        public string Thumbprint { get; set; }
    }
}
