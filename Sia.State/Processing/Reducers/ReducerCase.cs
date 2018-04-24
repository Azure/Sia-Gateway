using Sia.State.Filters;
using Sia.State.Generation.Transform;
using Sia.State.Processing.StateTransformTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Processing.Reducers
{
    public class ReducerCase<TState>
    {
        public EventFilters MatchTriggeringEvents { get; set; }
        public IStateTransformRule<TState> StateTransformToApply { get; set; }
    }
}
