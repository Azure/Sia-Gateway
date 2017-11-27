using Sia.Data.Incidents.Models;
using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Data.Incidents.Filters
{
    public class EventFilters: DataFilters<Event>
    {
        public long? IncidentId { get; set; }
        public long? EventTypeId { get; set; }
        public DateTime? Occurred { get; set; }
        public DateTime? EventFired { get; set; }


        public override IQueryable<Event> Filter(IQueryable<Event> source)
        {
            var working = source;

            if (IncidentId.HasValue) working = working.Where(ev => ev.IncidentId == IncidentId);
            if (EventTypeId.HasValue) working = working.Where(ev => ev.EventTypeId == EventTypeId);
            if (Occurred.HasValue) working = working.Where(ev => ev.Occurred == Occurred);
            if (EventFired.HasValue) working = working.Where(ev => ev.EventFired == EventFired);

            return base.Filter(working);
        }
    }
}
