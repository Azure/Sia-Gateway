using Sia.Data.Incidents.Models;
using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Data.Incidents.Filters
{
    public class EventFilters:Filters<Event>
    {
        public long? IncidentId { get; set; }
        public long? EventTypeId { get; set; }
        public DateTime? Occurred { get; set; }
        public DateTime? EventFired { get; set; }

        public override IQueryable<Event> Filter(IQueryable<Event> source)
            => source.Where(ev => !IncidentId.HasValue || ev.IncidentId == IncidentId)
                .Where(ev => !EventTypeId.HasValue || ev.EventTypeId == EventTypeId)
                .Where(ev => !Occurred.HasValue || ev.Occurred == Occurred)
                .Where(ev => !EventFired.HasValue || ev.EventFired == EventFired);
    }
}
