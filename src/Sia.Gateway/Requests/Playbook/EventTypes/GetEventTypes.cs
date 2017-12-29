using System.Collections.Generic;
using System.Net.Http;
using Sia.Domain.Playbook;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;

namespace Sia.Gateway.Requests
{
    public class GetEventTypesRequest : AuthenticatedRequest<IEnumerable<EventType>>
    {
        public GetEventTypesRequest(AuthenticatedUserContext userContext)
            : base(userContext)
        {
        }
    }

    public class GetEventTypesHandler : PlaybookProxyHandler<GetEventTypesRequest, IEnumerable<EventType>>
    {
        public GetEventTypesHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(GetEventTypesRequest request)
            => null;

        protected override HttpMethod Method()
            => HttpMethod.Get;

        protected override string RelativeUri(GetEventTypesRequest request)
            => "/eventTypes/";
    }
}
