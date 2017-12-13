using Microsoft.Extensions.Logging;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyConnector : Connector
    {
        public ProxyConnector(ProxyClient client, ILoggerFactory logger) 
            : base(client, logger)
        {
        }
    }
}
