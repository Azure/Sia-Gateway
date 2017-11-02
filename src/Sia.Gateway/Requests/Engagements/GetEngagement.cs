using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Shared.Authentication;
using Sia.Shared.Requests;
using System.Collections.Generic;
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
        public override async Task<Engagement> Handle(GetEngagementRequest request)
        {
            var EngagementRecord = await _context.Engagements
                .Include(en => en.Participant)
                .FirstOrDefaultAsync(ev => ev.IncidentId == request.IncidentId && ev.Id == request.Id);
            if (EngagementRecord == null) throw new KeyNotFoundException();

            return Mapper.Map<Engagement>(EngagementRecord);
        }
    }
}
