using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sia.Connectors.Tickets;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Core.Authentication;
using Sia.Core.Exceptions;
using Sia.Core.Requests;
using System.Collections.Generic;
using System.Threading;
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

    public class GetIncidentHandler
        : IncidentConnectorHandler<GetIncidentRequest, Incident>
    {
        
        public GetIncidentHandler(IncidentContext context, Connector connector)
            :base(context, connector){}

        public override async Task<Incident> Handle(GetIncidentRequest getIncident, CancellationToken cancellationToken)
        {
            var incidentRecord = await _context.Incidents
                .WithEagerLoading()
                .SingleOrDefaultAsync(cr => cr.Id == getIncident.Id, cancellationToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            if (incidentRecord == null) throw new NotFoundException($"Found no incident with id {getIncident.Id}.");

            var incident = Mapper.Map<Incident>(incidentRecord);

            AttachTickets(incident);

            return incident;
        }
    }
}
