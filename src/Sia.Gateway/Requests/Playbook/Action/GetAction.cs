using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;
using System.Net.Http;

namespace Sia.Playbook.Requests
{
    public class GetActionRequest : AuthenticatedRequest<Domain.Playbook.Action>
    {
        public GetActionRequest(long actionId, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            ActionId = actionId;
        }

        public long ActionId { get; private set; }
    }

    public class GetActionHandler : PlaybookProxyHandler<GetActionRequest, Domain.Playbook.Action>
    {
        public GetActionHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(GetActionRequest request) => null;
        protected override HttpMethod Method() => HttpMethod.Get;
        protected override string RelativeUri(GetActionRequest request)
            => $"/actions/{request.ActionId}";
    }
}
