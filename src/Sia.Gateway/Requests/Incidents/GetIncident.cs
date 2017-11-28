using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Connectors.Tickets;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Shared.Authentication;
using Sia.Shared.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class GetIncidentRequest : AuthenticatedRequest<Incident>
    {
        public GetIncidentRequest(long id, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            Id = id;
        }
        public long Id { get; }
    }

    public class GetIncidentHandler : IAsyncRequestHandler<GetIncidentRequest, Incident>
    {
        private readonly IncidentContext _context;
        private readonly Connector _connector;
        public GetIncidentHandler(IncidentContext context, Connector connector)
        {
            _context = context;
            _connector = connector;
        }
        public async Task<Incident> Handle(GetIncidentRequest getIncident)
        {
            var incidentRecord = await _context
                .Incidents
                .WithEagerLoading()
                .SingleOrDefaultAsync(cr => cr.Id == getIncident.Id);
            if (incidentRecord == null) throw new KeyNotFoundException();

            var incident = Mapper.Map<Incident>(incidentRecord);

            incident.Tickets = (await _connector
                    .GetData(incident.Tickets.AsEnumerable())
                ).ToList();

            return incident;
        }
    }
}
