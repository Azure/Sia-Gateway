using MediatR;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System.Threading.Tasks;
namespace Sia.Gateway.Requests
{
    public class PostEngagementRequest : AuthenticatedRequest, IRequest<Engagement>
    {
        public PostEngagementRequest(long incidentId, NewEngagement newEngagement, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            IncidentId = incidentId;
            NewEngagement = newEngagement;

        }
        public NewEngagement NewEngagement { get; }
        public long IncidentId { get; }
    }
    public class PostEngagementHandler : IAsyncRequestHandler<PostEngagementRequest, Engagement>
    {
        private IEngagementRepository _engagementRepository;

        public PostEngagementHandler(IEngagementRepository engagementRepository)
        {
            _engagementRepository = engagementRepository;
        }

        public async Task<Engagement> Handle(PostEngagementRequest request)
        {
            return await _engagementRepository.PostEngagementAsync(request.IncidentId, request.NewEngagement, request.UserContext);
        }
    }
};
