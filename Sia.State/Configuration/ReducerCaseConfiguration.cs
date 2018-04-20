using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Configuration
{
    public class ReducerCaseConfiguration
    {
        public EventShape TriggeringEventShape { get; set; }
        public StateTransformConfiguration StateTransformToApply { get; set; }
    }
}
