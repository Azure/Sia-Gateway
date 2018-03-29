using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Models
{
    public class EventShape
    {
        public long EventTypeId { get; set; }
        public IList<string> RequiredKeys { get; set; }
            = new List<string>();
        public ICollection<EventDataTransform> Transforms { get; set; }
            = new List<EventDataTransform>();
    }
}
