using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Gateway.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{

    public class GetEventRequest : AuthenticatedRequest, IRequest<Event>
    {
        public GetEventRequest(long incidentId, long id, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            IncidentId = incidentId;
            Id = id;
        }
        public long Id { get; }
        public long IncidentId { get; }
    }

    public class GetEventHandler : IAsyncRequestHandler<GetEventRequest, Event>
    {
        private readonly IncidentContext _context;

        public GetEventHandler(IncidentContext context)
        {
            _context = context;
        }
        public async Task<Event> Handle(GetEventRequest request)
        {
            var eventRecord = await _context
                                    .Events
                                    .FirstOrDefaultAsync( ev 
                                        => ev.IncidentId == request.IncidentId 
                                        && ev.Id == request.Id);
            if (eventRecord == null) throw new KeyNotFoundException();

            return Mapper.Map<Event>(eventRecord);
        }
    }
}
