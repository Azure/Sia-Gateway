using Microsoft.Extensions.Logging;

namespace Sia.Connectors.Tickets.None
{
    public class NoConnector : Connector
    {
        public NoConnector(NoClient client, ILoggerFactory loggerFactory)
            : base(client, loggerFactory)
        {
        }
    }
}
