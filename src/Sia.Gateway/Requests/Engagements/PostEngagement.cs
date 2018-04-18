using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Core.Authentication;
using Sia.Core.Exceptions;
using Sia.Core.Requests;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sia.Core.Validation;

namespace Sia.Gateway.Requests
{
    public class PostEngagementRequest : AuthenticatedRequest<Engagement>
    {
        public PostEngagementRequest(long incidentId, NewEngagement newEngagement, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            IncidentId = incidentId;
            NewEngagement = ThrowIf.Null(newEngagement, nameof(newEngagement));
        }
        public NewEngagement NewEngagement { get; }
        public long IncidentId { get; }
    }

    public class PostEngagementHandler
    : IncidentContextHandler<PostEngagementRequest, Engagement>
    {

        public PostEngagementHandler(IncidentContext context)
            :base(context)
        {
           
        }
        public override async Task<Engagement> Handle(PostEngagementRequest request, CancellationToken cancellationToken)
        {
            var dataIncident = await _context.Incidents
               .Include(cr => cr.Engagements)
                    .ThenInclude(en => en.Participant)
               .FirstOrDefaultAsync(x => x.Id == request.IncidentId, cancellationToken)
               .ConfigureAwait(continueOnCapturedContext: false);
            if (dataIncident == null) throw new NotFoundException($"Found no incident with id {request.IncidentId}");

            var dataEngagement = Mapper.Map<Data.Incidents.Models.Engagement>(request.NewEngagement);
            dataEngagement.TimeEngaged = DateTime.UtcNow;

            dataIncident.Engagements.Add(dataEngagement);
            await _context.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);

            return Mapper.Map<Engagement>(dataEngagement);
        }
    }
}
