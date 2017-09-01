using MediatR;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class PostIncidentRequest : AuthenticatedRequest, IRequest<Incident>
    {
        public PostIncidentRequest(NewIncident incident, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            Incident = incident;
        }

        public NewIncident Incident { get; private set; }

    }

    public class PostIncidentHandler : IAsyncRequestHandler<PostIncidentRequest, Incident>
    {
        private IIncidentRepository _incidentRepository;

        public PostIncidentHandler(IIncidentRepository incidentClient)
        {
            _incidentRepository = incidentClient;
        }
        public async Task<Incident> Handle(PostIncidentRequest message)
        {
            return await _incidentRepository.PostIncidentAsync(message.Incident, message.UserContext);
        }
    }
}
