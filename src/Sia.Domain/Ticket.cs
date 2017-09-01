namespace Sia.Domain
{
    public class Ticket
    {
        public long Id { get; set; }
        public long IncidentId { get; set; }
        public string OriginId { get; set; }
        public long TicketingSystemId { get; set; }
        public string OriginUri { get; set; }
    }
}
