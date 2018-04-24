using Sia.Data.Incidents.Models;
using Sia.State.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.State.Processing.Reducers
{
    public class CombinedReducer : IReducer
    {
        public string Name { get; set; }
        public IDictionary<string, object> InitialState { get; set; }
            = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public EventFilters ApplicableEvents { get; set; }
        public IDictionary<string, IReducer> Children { get; set; }
            = new Dictionary<string, IReducer>(StringComparer.InvariantCultureIgnoreCase);

        public object GetRawInitialState() => InitialState;
        public object UpdateSnapshot(IEnumerable<Event> candidateEvents, object currentState)
            => currentState == null
                ? UpdateSnapshot(candidateEvents, InitialState.AsEnumerable().ToDictionary()) // Copy of InitialState
                : UpdateSnapshot(candidateEvents, (IDictionary<string, object>)currentState);

        public IDictionary<string, object> UpdateSnapshot(IEnumerable<Event> candidateEvents, IDictionary<string, object> currentState)
        {
            foreach (var childReducer in Children)
            {
                var state = UpdateChildState(candidateEvents, currentState, childReducer);
                currentState.Remove(childReducer.Key);
                currentState.Add(childReducer.Key, state);
            }

            return currentState;
        }

        private static object UpdateChildState(IEnumerable<Event> candidateEvents, IDictionary<string, object> currentState, KeyValuePair<string, IReducer> childReducer)
        {
            object state;
            var applicableEvents = candidateEvents.Where(ev => childReducer.Value.ApplicableEvents.IsMatchFor(ev));
            if (currentState.TryGetValue(childReducer.Key, out var childState))
            {
                state = childReducer.Value.UpdateSnapshot(applicableEvents, childState);
            }
            else
            {
                state = childReducer.Value.UpdateSnapshot(applicableEvents, null); // From reducer initial state
            }

            return state;
        }
    }
}
