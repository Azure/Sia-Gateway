using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Connectors.Tickets;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Shared.Authentication;
using Sia.Shared.Requests;
using Sia.Shared.Protocol;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class GetIncidentsRequest : AuthenticatedRequest<IEnumerable<Incident>>
    {
        public GetIncidentsRequest(PaginationMetadata pagination, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            Pagination = pagination;
        }
        public PaginationMetadata Pagination { get; }
    }

    public class GetIncidentsHandler
        : IncidentConnectorHandler<GetIncidentsRequest, IEnumerable<Incident>>
    {
        public GetIncidentsHandler(
            IncidentContext context,
            Connector connector
        ) : base(context, connector) {}
        public override async Task<IEnumerable<Incident>> Handle(GetIncidentsRequest request, CancellationToken cancellationToken)
        {
            var incidentRecords = await _context.Incidents
                .WithEagerLoading()
                .WithPagination(request.Pagination)
                .ProjectTo<Incident>()
                .ToListAsync(cancellationToken);
            AttachTickets(incidentRecords);
            return incidentRecords;
        }
    }

}
