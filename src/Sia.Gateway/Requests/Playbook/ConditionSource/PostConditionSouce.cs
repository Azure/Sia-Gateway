using Sia.Domain.ApiModels.Playbooks;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;
using System.Net.Http;

namespace Sia.Gateway.Requests
{
    public class PostConditionSourceRequest : AuthenticatedRequest<Domain.Playbook.ConditionSource>
    {
        public PostConditionSourceRequest(CreateConditionSource createConditionSource, AuthenticatedUserContext userContext) : base(userContext)
        {
            CreateConditionSource = createConditionSource;
        }

        public CreateConditionSource CreateConditionSource { get; }
    }

    public class PostConditionSourceHandler : PlaybookProxyHandler<PostConditionSourceRequest, Domain.Playbook.ConditionSource>
    {
        public PostConditionSourceHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(PostConditionSourceRequest request)
            => request.CreateConditionSource;
        protected override HttpMethod Method()
            => HttpMethod.Post;
        protected override string RelativeUri(PostConditionSourceRequest request)
            => $"/conditionSources/";
    }
}
