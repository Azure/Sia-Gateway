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
    public class PostEventRequest : AuthenticatedRequest<Event>
    {
        public PostEventRequest(long incidentId, NewEvent newEvent, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            IncidentId = incidentId;
            NewEvent = ThrowIf.Null(newEvent, nameof(newEvent));
        }
        public NewEvent NewEvent { get; }
        public long IncidentId { get; }
    }

    public class PostEventHandler : IncidentContextHandler<PostEventRequest, Event>
    {
        public PostEventHandler(IncidentContext context)
            : base(context)
        {

        }
        public override async Task<Event> Handle(PostEventRequest request, CancellationToken cancellationToken)
        {
            var dataIncident = await _context
                                   .Incidents
                                   .Include(cr => cr.Events)
                                   .FirstOrDefaultAsync(x => x.Id == request.IncidentId, cancellationToken)
                                   .ConfigureAwait(continueOnCapturedContext: false);
            if (dataIncident == null) throw new NotFoundException($"Found no incident with id {request.IncidentId}.");

            var dataEvent = Mapper.Map<Data.Incidents.Models.Event>(request.NewEvent);

            dataIncident.Events.Add(dataEvent);
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

            return Mapper.Map<Event>(dataEvent);
        }
    }
}
