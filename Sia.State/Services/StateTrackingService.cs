using Sia.Core.Validation;
using Sia.Data.Incidents;
using Sia.State;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sia.State.Services
{
    public class StateTrackingService
    {
        private readonly ConcurrentDictionary<long, StateSnapshot> _backingCollection;
        private readonly StateGenerationService _reconciliationService;

        public StateTrackingService(
            ConcurrentDictionary<long, StateSnapshot> backingCollection,
            StateGenerationService reconciliationService
        )
        {
            _backingCollection = ThrowIf.Null(backingCollection, nameof(backingCollection));
            _reconciliationService = ThrowIf.Null(reconciliationService, nameof(reconciliationService));
        }

        public async Task<object> GetState(long incidentId)
        {
            if (_backingCollection.TryGetValue(incidentId, out StateSnapshot toReturn))
            {
                return toReturn.State;
            }

            return null;
        }

    }
}
