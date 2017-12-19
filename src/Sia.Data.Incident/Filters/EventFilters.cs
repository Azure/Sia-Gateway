using Sia.Data.Incidents.Models;
using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Sia.Data.Incidents.Filters
{
    public class EventFilters: DataFilters<Event>
    {
        public long? IncidentId { get; set; }
        public long[] EventTypes { get; set; }
        public DateTime? Occurred { get; set; }
        public DateTime? EventFired { get; set; }


        public override IQueryable<Event> Filter(IQueryable<Event> source)
        {
            var working = source;

            if (IncidentId.HasValue) working = working.Where(ev => ev.IncidentId == IncidentId);
            if (EventTypes != null && EventTypes.Length > 0) working = working.Where(ev => EventTypes.Contains(ev.EventTypeId));
            if (Occurred.HasValue) working = working.Where(ev => ev.Occurred == Occurred);
            if (EventFired.HasValue) working = working.Where(ev => ev.EventFired == EventFired);

            return base.Filter(working);
        }

        public override IEnumerable<KeyValuePair<string, string>> NonDataFilterValues()
        {
            if(IncidentId.HasValue) yield return new KeyValuePair<string, string>(nameof(IncidentId), IncidentId.Value.ToString());
            if(!(EventTypes is null) && EventTypes.Length != 0)
            {
                foreach (var eventTypeId in EventTypes)
                {
                    yield return new KeyValuePair<string, string>(nameof(EventTypes), eventTypeId.ToString());
                }
            }
            if (Occurred.HasValue) yield return new KeyValuePair<string, string>(nameof(Occurred), Occurred.Value.ToString());
            if (EventFired.HasValue) yield return new KeyValuePair<string, string>(nameof(EventFired), EventFired.Value.ToString());
        }
    }
}
