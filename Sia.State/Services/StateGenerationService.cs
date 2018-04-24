using Sia.Data.Incidents;
using Sia.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sia.State.Services
{
    public class StateGenerationService
    {
        private readonly IncidentContext _eventSource;
        private readonly IReducerService _reducerService;
        
        public StateGenerationService(
            IncidentContext eventSource,
            IReducerService reducerService
        )
        {
            _eventSource = ThrowIf.Null(eventSource, nameof(eventSource));
            _reducerService = ThrowIf.Null(reducerService, nameof(reducerService));
        }

        public Task<object> GenerateAsync(long incidentId)
        {
            // TODO: Implement
            return null;
        }

        public Task<object> ReconcileAsync(long incidentId, object partialState, DateTime asOf)
        {
            // TODO: Implement
            return null;
        }
    }
}
