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
    public class PostEventRequest : AuthenticatedRequest, IRequest<Event>
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

    public class PostEventHandler : IAsyncRequestHandler<PostEventRequest, Event>
    {
        private readonly IncidentContext _context;

        public PostEventHandler(IncidentContext context)
        {
            _context = context;
        }
        public async Task<Event> Handle(PostEventRequest request)
        {
            if (request.NewEvent == null) throw new ArgumentNullException(nameof(request.NewEvent));

            var dataCrisis = await _context
                                   .Incidents
                                   .Include(cr => cr.Events)
                                   .FirstOrDefaultAsync(x => x.Id == request.IncidentId);
            if (dataCrisis == null) throw new KeyNotFoundException();

            var dataEvent = Mapper.Map<Data.Incidents.Models.Event>(request.NewEvent);

            dataCrisis.Events.Add(dataEvent);
            await _context.SaveChangesAsync();

            return Mapper.Map<Event>(dataEvent);
        }
    }
}
