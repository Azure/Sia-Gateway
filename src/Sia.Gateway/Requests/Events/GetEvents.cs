using MediatR;
using Sia.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sia.Gateway.Authentication;
using Sia.Gateway.Protocol;
using Sia.Data.Incidents;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Sia.Gateway.Requests.Events
{
    public class GetEventsRequest : AuthenticatedRequest, IRequest<IEnumerable<Event>>
    {
        public GetEventsRequest(long incidentId, PaginationMetadata pagination, AuthenticatedUserContext userContext) 
            : base(userContext)
        {
            IncidentId = incidentId;
            Pagination = pagination;
        }

        public long IncidentId { get; }
        public PaginationMetadata Pagination { get; }
    }

    public class GetEventsHandler 
        : IAsyncRequestHandler<GetEventsRequest, IEnumerable<Event>>
    {
        private readonly IncidentContext _context;

        public GetEventsHandler(IncidentContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Event>> Handle(GetEventsRequest request)
                => await _context.Events
                .Where(ev => ev.IncidentId == request.IncidentId)
                .WithPagination(request.Pagination)
                .ProjectTo<Event>()
                .ToListAsync();
    }
}
