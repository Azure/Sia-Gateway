using Sia.Data.Incidents;
using Sia.Shared.Validation;
using Sia.State.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

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

        public object GetState(long incidentId)
        {
            var existingSnapshot
        }

    }
}
