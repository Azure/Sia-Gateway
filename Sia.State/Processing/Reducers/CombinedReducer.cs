using Sia.State.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Processing.Reducers
{
    public class CombinedReducer : IReducer
    {
        public IDictionary<string, object> InitialState { get; set; }
            = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public EventFilters ApplicableEvents { get; set; }
        public IDictionary<string, IReducer> Children { get; }
            = new Dictionary<string, IReducer>(StringComparer.InvariantCultureIgnoreCase);

        public object GetInitialState() => InitialState;
    }
}
