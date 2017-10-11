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
}
