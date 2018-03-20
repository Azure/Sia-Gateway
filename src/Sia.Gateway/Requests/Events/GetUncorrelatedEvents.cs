using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Gateway.Filters;
using Sia.Shared.Authentication;
using Sia.Shared.Protocol;
using Sia.Shared.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests.Events
{
    public class GetUncorrelatedEventsRequest : AuthenticatedRequest<IEnumerable<Event>>
    {
        public GetUncorrelatedEventsRequest(PaginationMetadata pagination,
            EventFilters filter,
            AuthenticatedUserContext userContext)
            : base(userContext)
        {
            Pagination = pagination;
            Filter = filter;
        }
        
        public PaginationMetadata Pagination { get; }
        public EventFilters Filter { get; }
    }

    public class GetUncorrelatedEventsHandler
        : IncidentContextHandler<GetUncorrelatedEventsRequest, IEnumerable<Event>>
    {
        public GetUncorrelatedEventsHandler(IncidentContext context)
            : base(context)
        {

        }
        public override async Task<IEnumerable<Event>> Handle(GetUncorrelatedEventsRequest request, CancellationToken cancellationToken)
            => await _context.Events
                .Include(ev => ev.Incident)
                    .ThenInclude(inc => inc.Tickets)
                .WithFilter(request.Filter)
                .WithPagination(request.Pagination)
                .ProjectTo<Event>()
                .ToListAsync(cancellationToken);
    }


}
