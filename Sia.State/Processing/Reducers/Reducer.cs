using Sia.Core.Validation;
using Sia.State.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Processing.Reducers
{
    public interface IReducer
    {
        object GetInitialState();
        EventFilters ApplicableEvents { get; }
    }
    public class Reducer<TState> : IReducer
        where TState : class
    {
        public TState InitialState { get; set; }
        public EventFilters ApplicableEvents { get; set; }
        public IList<ReducerCase<TState>> Cases { get; set; }

        public object GetInitialState() => InitialState;
    }
}
