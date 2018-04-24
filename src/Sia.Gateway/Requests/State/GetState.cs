using Microsoft.EntityFrameworkCore;
using Sia.Core.Authentication;
using Sia.Core.Requests;
using Sia.Data.Incidents;
using Sia.State.Processing.Reducers;
using Sia.State.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests.State
{
    public class GetStateRequest : AuthenticatedRequest<object>
    {
        public GetStateRequest(long incidentId, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            IncidentId = incidentId;
        }

        public long IncidentId { get; }
    }

    public class GetStateHandler : IncidentContextHandler<GetStateRequest, object>
    {
        public GetStateHandler(
            IncidentContext context,
            IReducerService reducerService
        ) : base (context)
        {
            ReducerService = reducerService;
        }

        public IReducerService ReducerService { get; }

        public override async Task<object> Handle(GetStateRequest message, CancellationToken cancellationToken)
        {
            IReducer reducer = await ReducerService.GetReducersAsync()
                .ConfigureAwait(continueOnCapturedContext: false);

            var eventsToAggregate = await _context.Events
                .Where(ev => ev.IncidentId.HasValue && ev.IncidentId == message.IncidentId)
                .WithFilter(reducer.ApplicableEvents)
                .ToListAsync()
                .ConfigureAwait(continueOnCapturedContext: false);

            return reducer.UpdateSnapshot(eventsToAggregate, null);
        }
    }
}
