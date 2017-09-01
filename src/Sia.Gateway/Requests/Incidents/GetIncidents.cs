using MediatR;
using Sia.Domain;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class GetIncidentsRequest : AuthenticatedRequest, IRequest<IEnumerable<Incident>>
    {
        public GetIncidentsRequest(AuthenticatedUserContext userContext) : base(userContext)
        {
        }
    }

    public class GetIncidentsHandler : IAsyncRequestHandler<GetIncidentsRequest, IEnumerable<Incident>>
    {
        private IIncidentRepository _incidentRepository;

        public GetIncidentsHandler(IIncidentRepository incidentClient)
        {
            _incidentRepository = incidentClient;
        }
        public async Task<IEnumerable<Incident>> Handle(GetIncidentsRequest message)
        {
            var incidentResponse = await _incidentRepository.GetIncidentsAsync(message.UserContext);
            return incidentResponse;
        }
    }
}
