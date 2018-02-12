using Sia.Connectors.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Initialization.Configuration
{
    public class GatewayConnectorsConfig
    {
        public TicketingConnectorConfig Ticket { get; set; }
    }
}
