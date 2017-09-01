using System.Collections.Generic;

namespace Sia.Domain
{
    public class Incident
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public Ticket PrimaryTicket { get; set; }
        public IList<Ticket> Tickets { get; set; }
            = new List<Ticket>();
        public IList<Event> Events { get; set; }
            = new List<Event>();
        public IList<Engagement> Engagements { get; set; }
            = new List<Engagement>();
    }
}
