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

    public class PostIncidentHandler
        : PostHandler<IIncidentRepository, PostIncidentRequest, Incident>
    {
        public PostIncidentHandler(IIncidentRepository repository)
            : base(repository)
        {
        }
    }
}
