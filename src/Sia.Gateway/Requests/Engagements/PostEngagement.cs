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
    public class PostEngagementHandler
        : PostHandler<IEngagementRepository, PostEngagementRequest, Engagement>
    {
        public PostEngagementHandler(IEngagementRepository repository)
            : base(repository)
        {
        }
    }
};
