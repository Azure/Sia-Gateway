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
using Sia.Data.Incidents.Filters;
using Sia.Shared.Protocol.Pagination;

namespace Sia.Gateway.Requests.Events
{
    public class GetEventsRequest : AuthenticatedRequest<IPaginationResultMetadata<Event>>
    {
        public GetEventsRequest(long incidentId,
            IPaginationRequest<Data.Incidents.Models.Event, Event> pagination,
            EventFilters filter,
            AuthenticatedUserContext userContext) 
            : base(userContext)
        {
            IncidentId = incidentId;
            Pagination = pagination;
            Filter = filter;
        }

        public long IncidentId { get; }
        public IPaginationRequest<Data.Incidents.Models.Event, Event> Pagination { get; }
        public EventFilters Filter { get; }
    }

    public class GetEventsHandler 
        : IncidentContextHandler<GetEventsRequest, IPaginationResultMetadata<Event>>
    {
        public GetEventsHandler(IncidentContext context)
            :base(context)
        {

        }
        public override async Task<IPaginationResultMetadata<Event>> Handle(GetEventsRequest request)
                => await _context.Events
                .Where(ev => ev.IncidentId == request.IncidentId)
                .WithFilter(request.Filter)
                .GetPaginatedResultAsync(request.Pagination);
    }
}
