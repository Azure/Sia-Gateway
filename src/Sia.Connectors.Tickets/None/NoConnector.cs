namespace Sia.Connectors.Tickets.None
{
    public class NoConnector : Connector<EmptyTicket>
    {
        public NoConnector(Client<EmptyTicket> client, Converter<EmptyTicket> converter)
            : base(client, converter)
        {
        }
    }
}
