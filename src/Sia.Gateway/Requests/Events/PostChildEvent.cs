using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Shared.Authentication;
using Sia.Shared.Exceptions;
using Sia.Shared.Requests;
using Sia.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class PostChildEventRequest : AuthenticatedRequest<Event>
    {
        public PostChildEventRequest(long incidentId, NewEvent newEvent, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            IncidentId = incidentId;
            NewEvent = ThrowIf.Null(newEvent, nameof(newEvent));

        }
        public NewEvent NewEvent { get; }
        public long IncidentId { get; }
    }

    public class PostChildEventHandler : IncidentContextHandler<PostChildEventRequest, Event>
    {
        public PostChildEventHandler(IncidentContext context)
            : base(context)
        {

        }
        public override async Task<Event> Handle(PostChildEventRequest request, CancellationToken cancellationToken)
        {
            var dataIncident = await _context
                                   .Incidents
                                   .Include(cr => cr.Events)
                                   .FirstOrDefaultAsync(x => x.Id == request.IncidentId, cancellationToken);
            if (dataIncident == null) throw new NotFoundException($"Could not find Incident with ID {request.IncidentId}");

            var dataEvent = Mapper.Map<Data.Incidents.Models.Event>(request.NewEvent);

            dataIncident.Events.Add(dataEvent);
            await _context.SaveChangesAsync(cancellationToken);

            return Mapper.Map<Event>(dataEvent);
        }
    }
}
