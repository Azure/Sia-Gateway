using System.Net.Http;
using Sia.Domain.Playbook;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;

namespace Sia.Playbook.Requests
{
    public class GetEventTypeRequest : AuthenticatedRequest<EventType>
    {
        public GetEventTypeRequest(long id, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            EventTypeId = id;
        }

        public long EventTypeId { get; private set; }
    }

    public class GetEventTypeHandler : PlaybookProxyHandler<GetEventTypeRequest, EventType>
    {
        public GetEventTypeHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(GetEventTypeRequest request)
            => null;
        protected override HttpMethod Method()
            => HttpMethod.Get;
        protected override string RelativeUri(GetEventTypeRequest request)
            => $"/eventTypes/{request.EventTypeId}";
    }
}
