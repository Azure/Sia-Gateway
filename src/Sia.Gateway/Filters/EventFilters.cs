using Sia.Data.Incidents.Models;
using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Sia.Domain;
using AutoMapper;

namespace Sia.Gateway.Filters
{
    public class EventFilters 
        : IFilters<Data.Incidents.Models.Event>,
        IFilters<Domain.Event>
    {
        public long? IncidentId { get; set; }
        public long[] EventTypes { get; set; }
        public DateTime? Occurred { get; set; }
        public DateTime? EventFired { get; set; }
        public string DataKey { get; set; }
        public string DataValue { get; set; }
        public const string KeyValueComparison = "\"{0}\":\"{1}\"";
        public const string KeyComparison = "\"{0}\":";

        public bool IsMatchFor(Data.Incidents.Models.Event toCompare)
        {
            if (IncidentId.HasValue && toCompare.IncidentId != IncidentId.Value) return false;
            if (EventTypes != null && EventTypes.Length > 0 && !EventTypes.Contains(toCompare.EventTypeId)) return false;
            if (Occurred.HasValue && toCompare.Occurred != Occurred.Value) return false;
            if (EventFired.HasValue && toCompare.EventFired != EventFired.Value) return false;

            if (!String.IsNullOrEmpty(DataKey))
            {
                if (String.IsNullOrEmpty(DataValue))
                {
                    if (!toCompare.Data.Contains(String.Format(KeyComparison, DataKey))) return false;
                }
                else
                {
                    if (!toCompare.Data.Contains(String.Format(KeyValueComparison, new string[] { DataKey, DataValue }))) return false;
                }
            }

            return true;
        }

        bool IFilterByMatch<Domain.Event>.IsMatchFor(Domain.Event toCompare)
            => IsMatchFor(Mapper.Map<Data.Incidents.Models.Event>(toCompare));

        /*public override IQueryable<Event> Filter(IQueryable<Event> source)
        {
            var working = source;

            if (IncidentId.HasValue) working = working.Where(ev => ev.IncidentId == IncidentId);
            if (EventTypes != null && EventTypes.Length > 0) working = working.Where(ev => EventTypes.Contains(ev.EventTypeId));
            if (Occurred.HasValue) working = working.Where(ev => ev.Occurred == Occurred);
            if (EventFired.HasValue) working = working.Where(ev => ev.EventFired == EventFired);

            if (!String.IsNullOrEmpty(DataKey))
            {
                var workingCompare = String.IsNullOrEmpty(DataValue)
                    ? String.Format(KeyComparison, DataKey)
                    : String.Format(KeyValueComparison, new string[] { DataKey, DataValue });
                working = working.Where(obj => obj.Data.Contains(workingCompare));
            }

            return working;
        }*/

        public IEnumerable<KeyValuePair<string, string>> FilterValues()
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

            if (!string.IsNullOrWhiteSpace(DataKey)) yield return new KeyValuePair<string, string>(nameof(DataKey), DataKey);
            if (!string.IsNullOrWhiteSpace(DataValue)) yield return new KeyValuePair<string, string>(nameof(DataValue), DataValue);
        }

        
    }
}
