using Sia.Connectors.Tickets.TicketProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Connectors.Tickets
{
    public class TicketingConnectorConfig
    {
        /// <summary>
        /// Path of assembly to load, if using ticketing connector from external assembly
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Configuraton settings related to the proxy.
        /// Only used if Path is null or empty.
        /// </summary>
        public ProxyConfig Proxy { get; set; }
    }
}
