using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using System;
using System.Threading.Tasks;


namespace Sia.Gateway.Requests
{
    public class PutEngagementRequest : AuthenticatedRequest, IRequest
    {
        public PutEngagementRequest(long incidentId, long engagementId, UpdateEngagement updateEngagement, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            IncidentId = incidentId;
            UpdateEngagement = updateEngagement;
            EngagementId = engagementId;
        }
        public UpdateEngagement UpdateEngagement { get; }
        public long EngagementId { get; }
        public long IncidentId { get; }
    }

    public class PutEngagementHandler
        : IAsyncRequestHandler<PutEngagementRequest>
    {
        private readonly IncidentContext _context;

        public PutEngagementHandler(IncidentContext context)
        {
            _context = context;
        }
        public async Task Handle(PutEngagementRequest request)
        {
            if (request.UpdateEngagement == null) throw new ArgumentNullException(nameof(UpdateEngagement));
            var existingRecord = await _context.Engagements
                .Include(en => en.Participant)
                .FirstOrDefaultAsync(engagement => engagement.IncidentId == request.IncidentId && engagement.Id == request.EngagementId);

            var updatedModel = Mapper.Map(request.UpdateEngagement, existingRecord);

            await _context.SaveChangesAsync();
        }
    }
}
