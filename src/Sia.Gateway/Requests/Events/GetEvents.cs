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
        public GetEventsRequest(long incidentId, PaginationMetadata pagination, AuthenticatedUserContext userContext) 
            : base(userContext)
        {
            IncidentId = incidentId;
            Pagination = pagination;
        }

        public long IncidentId { get; }
        public PaginationMetadata Pagination { get; }
    }
}
