using Sia.Data.Incidents.Models;
using System.Collections.Generic;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyConverter : Converter<ProxyTicket>
    {
        public override ICollection<Event> ExtractEvents(ProxyTicket ticket)
        {
            return new List<Event>();
        }
    }
}
