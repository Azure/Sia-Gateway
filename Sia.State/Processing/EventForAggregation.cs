namespace Sia.State.Processing
{
    using Newtonsoft.Json.Linq;
    using System;

    public class EventForAggregation
    {
        public long Id { get; set; }
        public long? IncidentId { get; set; }
        public long EventTypeId { get; set; }
        public DateTime Occurred { get; set; }
        public DateTime EventFired { get; set; }
        public string PrimaryTicketId { get; set; }
        public JObject Data { get; set; }
    }
}

namespace AutoMapper
{
    using Sia.Data.Incidents.Models;
    using Sia.State.Processing;

    public static class EventForMatchAutomapperExtensions
    {
        public static void MapBetweenEventAndEventForMatch(this IMapperConfigurationExpression configuration)
            => configuration.CreateMap<Event, EventForAggregation>();
    }
}