using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Gateway.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class GetIncidentsByTicketRequest : AuthenticatedRequest, IRequest<IEnumerable<Incident>>
    {
        public string TicketId { get; }

        public GetIncidentsByTicketRequest(string ticketId, AuthenticatedUserContext userContext) : base(userContext)
        {
            TicketId = ticketId;
        }
    }

    public class GetIncidentsByTicketHandler
    : IAsyncRequestHandler<GetIncidentsByTicketRequest, IEnumerable<Incident>>
    {
        private readonly IncidentContext _context;
        public GetIncidentsByTicketHandler(IncidentContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Incident>> Handle(GetIncidentsByTicketRequest request)
        {
            var incidentRecords = await _context.Incidents
                .WithEagerLoading()
                .Where(incident => incident.Tickets.Any(inc => inc.OriginId == request.TicketId))
                .ProjectTo<Incident>().ToListAsync();

            return incidentRecords;
        }
    }
}
