using Newtonsoft.Json.Linq;
using Sia.Data.Incidents.Models;
using Sia.State;
using Sia.State.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sia.State.Services
{
    public class StateGenerationImplementation
        : IStateGenerationImplementation
    {
        public async Task<JObject> GenerateStateAsync(
            object currentState,
            IEnumerable<EventForAggregation> orderedEvents,
            IEnumerable<ReducerCaseConfiguration> cases
        )
        {
            orderedEvents
                .Join(cases, (_) => true, (_) => true, (ev, rc) => new ReducerCaseChecker(rc, ev) )
                .Where(caseChecker => caseChecker.IsEventShapeMatch())
        }



        private class ReducerCaseChecker
        {
            public ReducerCaseChecker(ReducerCaseConfiguration rCase, EventForAggregation eMatch)
            {
                ReducerCase = rCase;
                EventForMatch = eMatch;
            }

            public ReducerCaseConfiguration ReducerCase { get; set; }
            public EventForAggregation EventForMatch { get; set; }

            public bool IsEventShapeMatch()
                => ReducerCase.TriggeringEventShapes.Any(shape => IsEventShapeMatch(shape, EventForMatch));
            private bool IsEventShapeMatch(EventShape shape, EventForAggregation toMatch)
                => shape.EventTypeId == toMatch.EventTypeId
                && shape.RequiredDataKeys
                    .All(key => !(toMatch.Data.SelectToken(key) == null));
        }
    }

    

    public interface IStateGenerationImplementation
    {
        Task<object> GenerateStateAsync(
            object currentState,
            IEnumerable<EventForAggregation> orderedEvents,
            IEnumerable<ReducerCaseConfiguration> cases
        );
    }
}
