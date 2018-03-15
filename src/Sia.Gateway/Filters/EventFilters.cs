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
<<<<<<< HEAD:src/Sia.Gateway/Filters/EventFilters.cs
    public class EventFilters 
        : IFilters<Data.Incidents.Models.Event>,
        IFilters<Domain.Event>
=======
    public class EventFilters : Filters<Event>
>>>>>>> master:src/Sia.Data.Incident/Filters/EventFilters.cs
    {
        public long? IncidentId { get; set; }
        public long[] EventTypes { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string DataKey { get; set; }
        public string DataValue { get; set; }
        public const string KeyValueComparison = "\"{0}\":\"{1}\"";
        public const string KeyComparison = "\"{0}\":";

        public bool IsMatchFor(Data.Incidents.Models.Event toCompare)
        {
<<<<<<< HEAD:src/Sia.Gateway/Filters/EventFilters.cs
            if (IncidentId.HasValue && toCompare.IncidentId != IncidentId.Value) return false;
            if (EventTypes != null && EventTypes.Length > 0 && !EventTypes.Contains(toCompare.EventTypeId)) return false;
            if (Occurred.HasValue && toCompare.Occurred != Occurred.Value) return false;
            if (EventFired.HasValue && toCompare.EventFired != EventFired.Value) return false;

=======
            var working = source;

            if (IncidentId.HasValue) working = working.Where(ev => ev.IncidentId == IncidentId);
            if (EventTypes != null && EventTypes.Length > 0) working = working.Where(ev => EventTypes.Contains(ev.EventTypeId));
            if (StartTime.HasValue) working = working.Where(ev => ev.Occurred.CompareTo(StartTime) > 0);
            if (EndTime.HasValue) working = working.Where(ev => ev.Occurred.CompareTo(EndTime) <= 0);
            
>>>>>>> master:src/Sia.Data.Incident/Filters/EventFilters.cs
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

        public IEnumerable<KeyValuePair<string, string>> FilterValues()
        {
            if (IncidentId.HasValue) yield return new KeyValuePair<string, string>(nameof(IncidentId), IncidentId.Value.ToString());
            if (!(EventTypes is null) && EventTypes.Length != 0)
            {
                foreach (var eventTypeId in EventTypes)
                {
                    yield return new KeyValuePair<string, string>(nameof(EventTypes), eventTypeId.ToString());
                }
            }
          
            if (!string.IsNullOrWhiteSpace(DataKey)) yield return new KeyValuePair<string, string>(nameof(DataKey), DataKey);
            if (!string.IsNullOrWhiteSpace(DataValue)) yield return new KeyValuePair<string, string>(nameof(DataValue), DataValue);
        }
    }
}
