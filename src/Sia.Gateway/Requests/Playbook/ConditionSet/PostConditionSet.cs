using Sia.Domain.ApiModels.Playbooks;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;
using System.Net.Http;

namespace Sia.Gateway.Requests
{
    public class PostConditionSetRequest : AuthenticatedRequest<Domain.Playbook.ConditionSet>
    {
        public PostConditionSetRequest(long actionId, CreateConditionSet createConditionSet, AuthenticatedUserContext userContext) : base(userContext)
        {
            CreateConditionSet = createConditionSet;
            ActionId = actionId;
        }

        public CreateConditionSet CreateConditionSet { get; }
        public long ActionId { get; }
    }

    public class PostConditionSetHandler : PlaybookProxyHandler<PostConditionSetRequest, Domain.Playbook.ConditionSet>
    {
        public PostConditionSetHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(PostConditionSetRequest request)
            => request.CreateConditionSet;
        protected override HttpMethod Method()
            => HttpMethod.Post;
        protected override string RelativeUri(PostConditionSetRequest request)
            => $"/actions/{request.ActionId}/conditionSets/";
    }
}
