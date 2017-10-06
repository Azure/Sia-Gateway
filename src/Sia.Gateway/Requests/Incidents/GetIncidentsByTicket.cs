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

    public class GetIncidentsByTicketHandler : IAsyncRequestHandler<GetIncidentsByTicketRequest, IEnumerable<Incident>>
    {
        private IIncidentRepository _incidentRepository;

        public GetIncidentsByTicketHandler(IIncidentRepository incidentClient)
        {
            _incidentRepository = incidentClient;
        }
        public async Task<IEnumerable<Incident>> Handle(GetIncidentsByTicketRequest message)
        {
            var incidentResponse = await _incidentRepository.GetManyAsync(message);
            return incidentResponse;
        }
    }
}
