using MediatR;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class PostEventRequest : AuthenticatedRequest, IRequest<Event>
    {
        public PostEventRequest(long incidentId, NewEvent newEvent, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            IncidentId = incidentId;
            NewEvent = newEvent;

        }
        public NewEvent NewEvent { get; }
        public long IncidentId { get; }
    }
    public class PostEventHandler : IAsyncRequestHandler<PostEventRequest, Event>
    {
        private IEventRepository _incidentRepository;

        public PostEventHandler(IEventRepository incidentRepository)
        {
            _incidentRepository = incidentRepository;
        }

        public async Task<Event> Handle(PostEventRequest request)
        {
            return await _incidentRepository.PostAsync(request);
        }
    }
}
