using MediatR;
using Sia.Domain;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{

    public class GetEventRequest : AuthenticatedRequest, IRequest<Event>
    {
        public GetEventRequest(long incidentId, long id, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            IncidentId = incidentId;
            Id = id;
        }
        public long Id { get; }
        public long IncidentId { get; }
    }
    public class GetEventHandler : IAsyncRequestHandler<GetEventRequest, Event>
    {
        private IEventRepository _incidentRepository;

        public GetEventHandler(IEventRepository incidentRepository)
        {
            _incidentRepository = incidentRepository;
        }
        public async Task<Event> Handle(GetEventRequest request)
        {
            return await _incidentRepository.GetEvent(request.IncidentId, request.Id, request.UserContext);
        }
    }

}
