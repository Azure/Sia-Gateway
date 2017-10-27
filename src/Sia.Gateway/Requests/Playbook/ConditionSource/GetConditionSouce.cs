using Sia.Domain.Playbook;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;
using System;
using System.Net.Http;

namespace Sia.Gateway.Requests
{
    public class GetConditionSourceRequest : AuthenticatedRequest<ConditionSource>
    {
        public GetConditionSourceRequest(long id, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            ConditionSourceId = id;
        }

        public long ConditionSourceId { get; private set; }
    }

    public class GetConditionSourceHandler : PlaybookProxyHandler<GetConditionSourceRequest, ConditionSource>
    {
        public GetConditionSourceHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(GetConditionSourceRequest request)
            => null;
        protected override HttpMethod Method()
            => HttpMethod.Get;
        protected override string RelativeUri(GetConditionSourceRequest request)
            => $"/conditionSources/{request.ConditionSourceId}";
    }
}
