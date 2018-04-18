using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Core.Authentication;
using Sia.Core.Exceptions;
using Sia.Core.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Sia.Gateway.Requests
{
    public class GetEngagementRequest : AuthenticatedRequest<Engagement>
    {
        public GetEngagementRequest(long incidentId, long id, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            IncidentId = incidentId;
            Id = id;
        }
        public long Id { get; }
        public long IncidentId { get; }
    }

    public class GetEngagementHandler
        : IncidentContextHandler<GetEngagementRequest, Engagement>
    {
        public GetEngagementHandler(IncidentContext context)
            :base(context)
        {

        }
        public override async Task<Engagement> Handle(GetEngagementRequest request, CancellationToken cancellationToken)
        {
            var EngagementRecord = await _context.Engagements
                .Include(en => en.Participant)
                .FirstOrDefaultAsync(ev => ev.IncidentId == request.IncidentId && ev.Id == request.Id, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            if (EngagementRecord == null) throw new NotFoundException($"Found no engagement with IncidentId {request.IncidentId} and Id {request.Id}!");

            return Mapper.Map<Engagement>(EngagementRecord);
        }
    }
}
