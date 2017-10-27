using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;
using System.Net.Http;

namespace Sia.Playbook.Requests
{
    public class GetConditionRequest : AuthenticatedRequest<Domain.Playbook.Condition>
    {
        public GetConditionRequest(long actionId, long conditionId, long conditionSetId, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            ActionId = actionId; 
            ConditionId = conditionId;
            ConditionSetId = conditionSetId;
        }

        public long ActionId { get; }
        public long ConditionId { get; }
        public long ConditionSetId { get; }
    }

    public class GetConditionHandler : PlaybookProxyHandler<GetConditionRequest, Domain.Playbook.Condition>
    {
        public GetConditionHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(GetConditionRequest request)
            => null;
        protected override HttpMethod Method()
            => HttpMethod.Get;
        protected override string RelativeUri(GetConditionRequest request)
            => $"/actions/{request.ActionId}/conditionSets/{request.ConditionSetId}/condtions/{request.ConditionId}";
    }
}
