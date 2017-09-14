using Sia.Shared.Data;
using System.Collections.Generic;

namespace Sia.Domain
{
    public class Incident : IEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public Ticket PrimaryTicket { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
            = new List<Ticket>();
        public ICollection<Event> Events { get; set; }
            = new List<Event>();
        public ICollection<Engagement> Engagements { get; set; }
            = new List<Engagement>();
    }
}
