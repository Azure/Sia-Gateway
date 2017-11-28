using Microsoft.Extensions.Logging;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyConnector : Connector
    {
        public ProxyConnector(ProxyClient client, ILogger logger) 
            : base(client, logger)
        {
        }
    }
}
