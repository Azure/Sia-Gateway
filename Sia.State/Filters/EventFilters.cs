using AutoMapper;
using Sia.Core.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sia.State.Filters
{

    public class EventFilters 
        : IFilters<Data.Incidents.Models.Event>,
        IFilters<Domain.Event>
    {
        public long? IncidentId { get; set; }
        public List<long> EventTypes { get; set; }
            = new List<long>();
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public IList<FilterKeyValuePair> MatchesAny { get; set; }
            = new List<FilterKeyValuePair>();
        public string DataKey { get; set; }
        public string DataValue { get; set; }
        public string DataSearch { get; set; }

        public const string KeyValueComparison = "\"{0}\":\"{1}\"";
        public const string KeyComparison = "\"{0}\":";

        public bool IsMatchFor(Data.Incidents.Models.Event toCompare)
        {
            if (IncidentId.HasValue && toCompare.IncidentId != IncidentId.Value) { return false; }
            if (EventTypes != null && EventTypes.Count > 0 && !EventTypes.Contains(toCompare.EventTypeId)) { return false; }
            if (StartTime.HasValue && toCompare.Occurred.CompareTo(StartTime) <= 0) { return false; }
            if (EndTime.HasValue && toCompare.Occurred.CompareTo(EndTime) > 0) { return false; }

            if (!MatchesKeyValuePair(toCompare, DataKey, DataValue)) { return false; }

            if (!MatchesAny.Any(pair => MatchesKeyValuePair(toCompare, pair.Key, pair.Value))) { return false; }

            if (!string.IsNullOrEmpty(DataSearch) && (toCompare.Data == null || !toCompare.Data.Contains(DataSearch))) { return false; }

            return true;
        }

        bool IFilterByMatch<Domain.Event>.IsMatchFor(Domain.Event toCompare)
            => IsMatchFor(Mapper.Map<Data.Incidents.Models.Event>(toCompare));

        public IEnumerable<KeyValuePair<string, string>> FilterValues()
        {
            if (IncidentId.HasValue) { yield return new KeyValuePair<string, string>(nameof(IncidentId), IncidentId.Value.ToPathTokenString()); }
            if (!(EventTypes is null) && EventTypes.Count != 0)
            {
                foreach (var eventTypeId in EventTypes)
                {
                    yield return new KeyValuePair<string, string>(nameof(EventTypes), eventTypeId.ToPathTokenString());
                }
            }
            if (StartTime.HasValue) { yield return new KeyValuePair<string, string>(nameof(StartTime), StartTime.Value.ToPathTokenString()); }
            if (EndTime.HasValue) { yield return new KeyValuePair<string, string>(nameof(EndTime), EndTime.Value.ToPathTokenString()); }

            if (!string.IsNullOrWhiteSpace(DataKey)) { yield return new KeyValuePair<string, string>(nameof(DataKey), DataKey); }
            if (!string.IsNullOrWhiteSpace(DataValue)) { yield return new KeyValuePair<string, string>(nameof(DataValue), DataValue); }
        }

        private static bool MatchesKeyValuePair(Data.Incidents.Models.Event toCompare, string key, string value)
        {
            if (!String.IsNullOrEmpty(key))
            {
                if (String.IsNullOrEmpty(value))
                {
                    if (!toCompare.Data.Contains(String.Format(CultureInfo.InvariantCulture, KeyComparison, key))) { return false; }
                }
                else
                {
                    if (!toCompare.Data.Contains(String.Format(CultureInfo.InvariantCulture, KeyValueComparison, new string[] { key, value }))) { return false; }
                }
            }
            return true;
        }
    }
}
