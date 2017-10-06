using MediatR;
using Sia.Domain;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class GetIncidentsByTicketRequest : AuthenticatedRequest, IRequest<IEnumerable<Incident>>
    {
        public string TicketId { get; }

        public GetIncidentsByTicketRequest(string ticketId, AuthenticatedUserContext userContext) : base(userContext)
        {
            TicketId = ticketId;
        }
    }

    public class GetIncidentsByTicketHandler
        : GetManyHandler<IIncidentRepository, GetIncidentsByTicketRequest, Incident>
    {
        public GetIncidentsByTicketHandler(IIncidentRepository repository)
            : base(repository)
        {
        }
    }
}
