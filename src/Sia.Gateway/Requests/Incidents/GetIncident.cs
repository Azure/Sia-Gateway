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
    public class GetIncidentHandler : IAsyncRequestHandler<GetIncidentRequest, Incident>
    {
        private IIncidentRepository _incidentRepository;

        public GetIncidentHandler(IIncidentRepository incidentRepository)
        {
            _incidentRepository = incidentRepository;
        }

        public async Task<Incident> Handle(GetIncidentRequest request)
        {
            return await _incidentRepository.GetAsync(request);
        }
    }
}
