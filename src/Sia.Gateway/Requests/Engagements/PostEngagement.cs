using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class PostEngagementRequest : AuthenticatedRequest, IRequest<Engagement>
    {
        public PostEngagementRequest(long incidentId, NewEngagement newEngagement, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            IncidentId = incidentId;
            NewEngagement = newEngagement;

        }
        public NewEngagement NewEngagement { get; }
        public long IncidentId { get; }
    }

    public class PostEngagementHandler
    : IAsyncRequestHandler<PostEngagementRequest, Engagement>
    {
        private readonly IncidentContext _context;

        public PostEngagementHandler(IncidentContext context)
        {
            _context = context;
        }
        public async Task<Engagement> Handle(PostEngagementRequest request)
        {
            if (request.NewEngagement == null) throw new ArgumentNullException(nameof(request.NewEngagement));

            var dataIncident = await _context.Incidents
               .Include(cr => cr.Engagements)
                    .ThenInclude(en => en.Participant)
               .FirstOrDefaultAsync(x => x.Id == request.IncidentId);
            if (dataIncident == null) throw new KeyNotFoundException();

            var dataEngagement = Mapper.Map<Data.Incidents.Models.Engagement>(request.NewEngagement);
            dataEngagement.TimeEngaged = DateTime.UtcNow;

            dataIncident.Engagements.Add(dataEngagement);
            await _context.SaveChangesAsync();

            return Mapper.Map<Engagement>(dataEngagement);
        }
    }
}
