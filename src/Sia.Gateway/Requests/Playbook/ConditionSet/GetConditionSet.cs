using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;
using System.Net.Http;

namespace Sia.Playbook.Requests
{
    public class GetConditionSetRequest : AuthenticatedRequest<Domain.Playbook.ConditionSet>
    {
        public GetConditionSetRequest(long conditionSetId, long actionId, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            ConditionSetId = conditionSetId;
            ActionId = actionId;
        }

        public long ConditionSetId { get; private set; }
        public long ActionId { get; }
    }

    public class GetConditionSetHandler : PlaybookProxyHandler<GetConditionSetRequest, Domain.Playbook.ConditionSet>
    {
        public GetConditionSetHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(GetConditionSetRequest request)
            => null;
        protected override HttpMethod Method()
            => HttpMethod.Get;
        protected override string RelativeUri(GetConditionSetRequest request)
            => $"/actions/{request.ActionId}/conditionSets/{request.ConditionSetId}/";
    }
}
