using MediatR;
using Sia.Domain;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class GetIncidentRequest : AuthenticatedRequest, IRequest<Incident>
    {
        public GetIncidentRequest(long id, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            Id = id;
        }
        public long Id { get; }
    }
    public class GetIncidentHandler :
        GetHandler<IIncidentRepository, GetIncidentRequest, Incident>
    {
        public GetIncidentHandler(IIncidentRepository repository)
            : base(repository)
        {
        }
    }
}
