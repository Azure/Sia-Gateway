using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Configuration
{
    public class EventShape
    {
        public long EventTypeId { get; set; }
        public IList<string> RequiredDataKeys { get; set; }
            = new List<string>();
        public IList<EventDataTransform> Transforms { get; set; }
            = new List<EventDataTransform>();
    }
}
