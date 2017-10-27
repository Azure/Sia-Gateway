using Sia.Domain.ApiModels.Playbooks;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;
using System.Net.Http;

namespace Sia.Playbook.Requests
{
    public class PostConditionRequest : AuthenticatedRequest<Domain.Playbook.Condition>
    {
        public PostConditionRequest(long actionId, long conditionSetId, CreateCondition createCondition, AuthenticatedUserContext userContext) : base(userContext)
        {
            ActionId = actionId;
            CreateCondition = createCondition;
            ConditionSetId = conditionSetId;
        }

        public long ActionId { get; }
        public CreateCondition CreateCondition { get; }
        public long ConditionSetId { get; }
    }

    public class PostConditionHandler : PlaybookProxyHandler<PostConditionRequest, Domain.Playbook.Condition>
    {
        public PostConditionHandler(HttpClientLookup clientFactory) 
            : base(clientFactory)
        {
        }

        protected override object MessageContent(PostConditionRequest request)
            => request.CreateCondition;
        protected override HttpMethod Method()
            => HttpMethod.Post;
        protected override string RelativeUri(PostConditionRequest request)
            => $"/actions/{request.ActionId}/conditionSets/{request.ConditionSetId}/condtions/";
    }
}
