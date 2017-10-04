using Sia.Shared.Data;
using System;

namespace Sia.Data.Incidents.Models
{
    public class Event : IEntity
    {
        public long Id { get; set; }
        public long? IncidentId { get; set; }
        public long EventTypeId { get; set; }
        public DateTime Occurred { get; set; }
        public DateTime EventFired { get; set; }
    }
}
