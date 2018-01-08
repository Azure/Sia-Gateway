using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Shared.Authentication;
using Sia.Shared.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{

    public class GetEventRequest : AuthenticatedRequest<Event>
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

    public class GetEventHandler 
        : IncidentContextHandler<GetEventRequest, Event>
    {
        public GetEventHandler(IncidentContext context)
            :base(context)
        {

        }
        public override async Task<Event> Handle(GetEventRequest request, CancellationToken cancellationToken)
        {
            var eventRecord = await _context
                                    .Events
                                    .FirstOrDefaultAsync( ev 
                                        => ev.IncidentId == request.IncidentId 
                                        && ev.Id == request.Id,
                                        cancellationToken);
            if (eventRecord == null) throw new KeyNotFoundException();

            return Mapper.Map<Event>(eventRecord);
        }
    }
}
