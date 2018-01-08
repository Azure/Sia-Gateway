using System.Collections.Generic;
using System.Net.Http;
using Sia.Domain.Playbook;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;

namespace Sia.Gateway.Requests
{
    public class GetGlobalActionsRequest : AuthenticatedRequest<IEnumerable<Action>>
    {
        public GetGlobalActionsRequest(AuthenticatedUserContext userContext)
            : base(userContext)
        {
        }
    }

    public class GetGlobalActionsHandler : PlaybookProxyHandler<GetGlobalActionsRequest, IEnumerable<Action>>
    {
        public GetGlobalActionsHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(GetGlobalActionsRequest request)
            => null;

        protected override HttpMethod Method()
            => HttpMethod.Get;

        protected override string RelativeUri(GetGlobalActionsRequest request)
            => "/globalActions/";
    }
}
