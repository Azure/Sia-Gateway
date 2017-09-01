namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyConnector : Connector<Ticket>
    {
        public ProxyConnector(Client<Ticket> client, Converter<Ticket> converter) 
            : base(client, converter)
        {
        }
    }
}
