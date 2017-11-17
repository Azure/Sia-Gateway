using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Shared.Authentication;
using Sia.Shared.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class PostEventRequest : AuthenticatedRequest<Event>
    {
        public PostEventRequest(long incidentId, NewEvent newEvent, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            IncidentId = incidentId;
            NewEvent = newEvent;

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
        public override async Task<Event> Handle(PostEventRequest request)
        {
            if (request.NewEvent == null) throw new ArgumentNullException(nameof(request.NewEvent));

            var dataIncident = await _context
                                   .Incidents
                                   .Include(cr => cr.Events)
                                   .FirstOrDefaultAsync(x => x.Id == request.IncidentId);
            if (dataIncident == null) throw new KeyNotFoundException();

            var dataEvent = Mapper.Map<Data.Incidents.Models.Event>(request.NewEvent);

            dataIncident.Events.Add(dataEvent);
            await _context.SaveChangesAsync();

            return Mapper.Map<Event>(dataEvent);
        }
    }
}
