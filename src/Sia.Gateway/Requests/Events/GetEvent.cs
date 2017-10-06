using MediatR;
using Sia.Domain;
using Sia.Gateway.Authentication;
using Sia.Gateway.Requests.Events;
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
    public class GetEventHandler : EventHandler<GetEventRequest, Event>
    {
        public GetEventHandler(IEventRepository eventRepository) 
            : base(eventRepository)
        {
        }

        public override async Task<Event> Handle(GetEventRequest request)
        {
            return await _eventRepository.GetAsync(request);
        }
    }

}
