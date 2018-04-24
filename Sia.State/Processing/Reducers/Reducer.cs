using Sia.Core.Validation;
using Sia.Data.Incidents.Models;
using Sia.State.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.State.Processing.Reducers
{
    public interface IReducer
    {
        string Name { get; set; }
        object GetRawInitialState();
        EventFilters ApplicableEvents { get; }
        object UpdateSnapshot(IEnumerable<Event> candidateEvents, object currentState);
    }
    public class Reducer<TState> : IReducer
        where TState : class
    {
        public string Name { get; set; }
        public TState InitialState { get; set; }
        public EventFilters ApplicableEvents { get; set; }
        public IList<ReducerCase<TState>> Cases { get; set; }

        public object GetRawInitialState() => InitialState;
        public object UpdateSnapshot(IEnumerable<Event> candidateEvents, object currentState)
            => currentState == null
                ? UpdateSnapshot(candidateEvents, InitialState)
                : UpdateSnapshot(candidateEvents, (TState)currentState);

        public TState UpdateSnapshot(IEnumerable<Event> candidateEvents, TState currentState)
        {
            var workingState = currentState;
            var transforms = candidateEvents
                .SelectMany(ev => Cases
                    .Where(rCase => rCase.MatchTriggeringEvents.IsMatchFor(ev))
                    .Select(rCase => (transform: rCase.StateTransformToApply.GetTransform(ev), ev: ev)));

            ApplyTransforms(ref workingState, transforms);

            return workingState;
        }

        private void ApplyTransforms(ref TState currentState, IEnumerable<(Generation.Transform.IStateTransform<TState> transform, Event ev)> transforms)
        {
            long currentEventId = 0;
            try
            {
                foreach (var (transform, ev) in transforms)
                {
                    currentEventId = ev.Id;
                    transform.Apply(ref currentState);
                }
            }
            catch (Exception ex)
            {
                throw new ReducerApplicationFailureException(ex, Name, currentEventId);
            }
        }
    }
}
