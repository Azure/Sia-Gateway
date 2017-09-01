using Sia.Data.Incidents.Models;
using System.Collections.Generic;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyConverter : Converter<Ticket>
    {
        public override ICollection<Event> ExtractEvents(Ticket ticket)
        {
            return new List<Event>();
        }
    }
}
