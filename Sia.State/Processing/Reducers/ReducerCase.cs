using Sia.State.Filters;
using Sia.State.Processing.Transforms;

namespace Sia.State.Processing.Reducers
{
    public class ReducerCase<TState>
    {
        public EventFilters MatchTriggeringEvents { get; set; }
        public IStateTransformRule<TState> StateTransformToApply { get; set; }
    }
}
