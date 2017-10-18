namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyConnector : Connector<ProxyTicket>
    {
        public ProxyConnector(Client<ProxyTicket> client, Converter<ProxyTicket> converter) 
            : base(client, converter)
        {
        }
    }
}
