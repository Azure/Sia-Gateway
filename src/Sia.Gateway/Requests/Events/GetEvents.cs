using MediatR;
using Sia.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sia.Gateway.Authentication;
using Sia.Gateway.Protocol;
using Sia.Gateway.ServiceRepositories;

namespace Sia.Gateway.Requests.Events
{
    public class GetEventsRequest : AuthenticatedRequest, IRequest<IEnumerable<Event>>
    {
        protected GetEventsRequest(long incidentId, PaginationMetadata pagination, AuthenticatedUserContext userContext) 
            : base(userContext)
        {
            IncidentId = incidentId;
            Pagination = pagination;
        }

        public long IncidentId { get; }
        public PaginationMetadata Pagination { get; }
    }

    public class GetEventsHandler : EventHandler<GetEventsRequest, IEnumerable<Event>>
    {
        protected GetEventsHandler(IEventRepository eventRepository) : base(eventRepository)
        {
        }

        public override Task<IEnumerable<Event>> Handle(GetEventsRequest request)
        {
            return await _eventRepository
        }
    }
}
