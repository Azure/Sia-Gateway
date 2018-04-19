using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Models.Configuration
{
    public class ReducerCase
    {
        public IList<EventShape> TriggeringEventShapes { get; set; }
            = new List<EventShape>();
        public IList<StateTransform> StateTransformsToApply { get; set; }
            = new List<StateTransform>();
    }
}
