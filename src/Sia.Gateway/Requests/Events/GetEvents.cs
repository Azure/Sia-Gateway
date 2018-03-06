using MediatR;
using Sia.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sia.Shared.Authentication;
using Sia.Shared.Protocol;
using Sia.Data.Incidents;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sia.Shared.Requests;
using Sia.Shared.Data;
using System.Threading;
using Sia.Gateway.Filters;

namespace Sia.Gateway.Requests.Events
{
    public class GetEventsRequest : AuthenticatedRequest<IEnumerable<Event>>
    {
        public GetEventsRequest(long incidentId,
            PaginationMetadata pagination,
            EventFilters filter,
            AuthenticatedUserContext userContext) 
            : base(userContext)
        {
            IncidentId = incidentId;
            Pagination = pagination;
            Filter = filter;
        }

        public long IncidentId { get; }
        public PaginationMetadata Pagination { get; }
        public EventFilters Filter { get; }
    }

    public class GetEventsHandler 
        : IncidentContextHandler<GetEventsRequest, IEnumerable<Event>>
    {
        public GetEventsHandler(IncidentContext context)
            :base(context)
        {

        }
        public override async Task<IEnumerable<Event>> Handle(GetEventsRequest request, CancellationToken cancellationToken)
                => await _context.Events
                .Where(ev => ev.IncidentId == request.IncidentId)
                .WithFilter(request.Filter)
                .WithPagination(request.Pagination)
                .ProjectTo<Event>()
                .ToListAsync(cancellationToken);
    }
}
