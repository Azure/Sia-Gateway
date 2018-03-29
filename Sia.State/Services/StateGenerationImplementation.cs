using Sia.Domain;
using Sia.State.Models;
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
        public async Task<object> GenerateStateAsync(
            object currentState,
            IEnumerable<Event> orderedEvents,
            IEnumerable<ReducerCase> cases
        )
        {

        }

        private bool IsEventShapeMatch(EventShape shape, Event toMatch)
            => shape.
    }
    public interface IStateGenerationImplementation
    {
        Task<object> GenerateStateAsync(
            object currentState,
            IEnumerable<Event> orderedEvents,
            IEnumerable<ReducerCase> cases
        );
    }
}
