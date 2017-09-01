namespace Sia.Connectors.Tickets.TicketProxy
{
    public class Ticket
    {
        public string OriginId { get; set; }
        public long IncidentSystemId { get; set; }
        public string OriginUri { get; set; }
        public string Title { get; set; }
        public string Owner { get; set; }
        public string Severity { get; set; }
    }
}
