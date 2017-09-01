using Sia.Data.Incidents.Models;
using System.Collections.Generic;

namespace Sia.Connectors.Tickets.None
{
    public class NoConverter : Converter<EmptyTicket>
    {
        public override ICollection<Event> ExtractEvents(EmptyTicket ticket)
        {
            return new List<Event>();
        }
    }
}
