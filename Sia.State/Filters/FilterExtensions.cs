using Sia.State.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.State.Configuration.Models
{
    public static class FilterExtensions
    {
        /// <summary>
        /// Returns a filter that matches any event that any of the input filters would match.
        /// TODO: Optimize, actually take DataKey/Value/Search into account
        /// </summary>
        public static EventFilters ToUnionFilter(this IEnumerable<EventFilters> filters)
            => new EventFilters()
            {
                EventTypes = filters
                    .SelectMany(filter => filter.EventTypes)
                    .ToHashSet() // Unique
                    .ToList(),
                StartTime = filters
                    .Select(filter => filter.StartTime)
                    .Any(time => time.HasValue)
                    ? new DateTime?(filters
                        .Select(filter => filter.StartTime)
                        .Where(time => time.HasValue)
                        .Min(time => time.Value))
                    : null,
                EndTime = filters
                    .Select(filter => filter.EndTime)
                    .Any(time => time.HasValue)
                    ? new DateTime?(filters
                        .Select(filter => filter.EndTime)
                        .Where(time => time.HasValue)
                        .Max(time => time.Value))
                    : null,
                RequiredDataKeys = filters
                    .SelectMany(filter => filter.RequiredDataKeys)
                    .ToHashSet(StringComparer.InvariantCultureIgnoreCase) // Unique
                    .ToList()
            };

        public static EventFilters ToFilter(this EventShape shape)
            => new EventFilters()
            {
                EventTypes = new List<long>() { shape.EventTypeId },
                RequiredDataKeys = shape.RequiredDataKeys
            };
    }
}
