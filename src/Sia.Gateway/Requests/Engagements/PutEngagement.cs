﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain.ApiModels;
using Sia.Core.Authentication;
using Sia.Core.Exceptions;
using Sia.Core.Requests;
using System;
using System.Collections.Generic;
using System.Threading;
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
        : IncidentContextHandler<PutEngagementRequest>
    { 

        public PutEngagementHandler(IncidentContext context)
            :base(context)
        {

        }
        public override async Task Handle(PutEngagementRequest request, CancellationToken cancellationToken)
        {
            if (request.UpdateEngagement is null) throw new ArgumentNullException(nameof(UpdateEngagement));
            var existingRecord = await _context.Engagements
                .Include(en => en.Participant)
                .FirstOrDefaultAsync(engagement => engagement.IncidentId == request.IncidentId && engagement.Id == request.EngagementId, cancellationToken);
            if (existingRecord is null) throw new NotFoundException($"Found no engagement with incidentId {request.IncidentId} and id {request.EngagementId}.");

            var updatedModel = Mapper.Map(request.UpdateEngagement, existingRecord);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
