using Sia.Domain;
using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Sia.Domain
{
    public class Event : IEntity, IDynamicDataSource
    {
        public long Id { get; set; }
        public long? IncidentId { get; set; }
        public long EventTypeId { get; set; }
        public DateTime Occurred { get; set; }
        public DateTime EventFired { get; set; }
        public dynamic Data { get; set; } = new ExpandoObject();
    }
}
