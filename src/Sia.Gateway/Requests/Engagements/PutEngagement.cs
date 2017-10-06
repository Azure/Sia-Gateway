using MediatR;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class PutEngagementRequest : AuthenticatedRequest, IRequest
    {
        public PutEngagementRequest(long incidentId, long engagementId, UpdateEngagement updateEngagement, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            IncidentId = incidentId;
            UpdateEngagement = updateEngagement;
            EngagementId = engagementId;
        }
        public UpdateEngagement UpdateEngagement { get; }
        public long EngagementId { get; }
        public long IncidentId { get; }
    }
    public class PutEngagementHandler : IAsyncRequestHandler<PutEngagementRequest>
    {
        private IEngagementRepository _engagementClient;

        public PutEngagementHandler(IEngagementRepository incidentClient)
        {
            _engagementClient = incidentClient;
        }

        public async Task Handle(PutEngagementRequest request)
        {
            await _engagementClient.PutAsync(request);
        }
    }
}
