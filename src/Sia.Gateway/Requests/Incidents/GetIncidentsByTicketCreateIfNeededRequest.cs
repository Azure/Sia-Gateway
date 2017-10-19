using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class GetIncidentsByTicketCreateIfNeededRequest : AuthenticatedRequest, IRequest<IEnumerable<Incident>>
    {
        public GetIncidentsByTicketCreateIfNeededRequest(string ticketId, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            TicketId = ticketId;
        }

        public string TicketId { get; private set; }

    }

    public class GetIncidentsByTicketCreateIfNeededRequestHandler : IAsyncRequestHandler<GetIncidentsByTicketCreateIfNeededRequest, IEnumerable<Incident>>
    {

        private IncidentContext _context;

        public GetIncidentsByTicketCreateIfNeededRequestHandler(IncidentContext context)
        {
            _context = context;
        }

        public IncidentContext Context { get; }

        public async Task<IEnumerable<Incident>> Handle(GetIncidentsByTicketCreateIfNeededRequest message)
        {
            var incidents = await _context.Incidents
                .WithEagerLoading()
                .Where(incident => incident.Tickets.Any(inc => inc.OriginId == message.TicketId))
                .ProjectTo<Incident>().ToListAsync();

            if (incidents.Any())
            {
                return incidents;
            }

            var primaryTicket = new Ticket
            {
                TicketingSystemId = 1,
                OriginId = message.TicketId
            };
            var newIncident = new NewIncident
            {
                PrimaryTicket = primaryTicket,
                Tickets = new List<Ticket> {
                        primaryTicket
                    }
            };

            var dataIncident = Mapper.Map<Data.Incidents.Models.Incident>(newIncident);

            var result = _context.Incidents.Add(dataIncident);
            await _context.SaveChangesAsync();

            return new List<Incident> { Mapper.Map<Incident>(result.Entity) };
        }
    }
}
