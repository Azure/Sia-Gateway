using Sia.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Domain
{
    public class Event
    {
        public long Id { get; set; }
        public long? IncidentId { get; set; }
        public long EventTypeId { get; set; }
        public DateTime Occurred { get; set; }
        public DateTime EventFired { get; set; }
    }
}
