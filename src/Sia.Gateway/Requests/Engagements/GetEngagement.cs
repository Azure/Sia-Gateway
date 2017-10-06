using MediatR;
using Sia.Domain;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class GetEngagementRequest : AuthenticatedRequest, IRequest<Engagement>
    {
        public GetEngagementRequest(long incidentId, long id, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            IncidentId = incidentId;
            Id = id;
        }
        public long Id { get; }
        public long IncidentId { get; }
    }
    public class GetEngagementHandler : IAsyncRequestHandler<GetEngagementRequest, Engagement>
    {
        private IEngagementRepository _incidentRepository;

        public GetEngagementHandler(IEngagementRepository incidentRepository)
        {
            _incidentRepository = incidentRepository;
        }
        public async Task<Engagement> Handle(GetEngagementRequest request)
        {
            return await _incidentRepository.GetAsync(request);
        }
    }
}
