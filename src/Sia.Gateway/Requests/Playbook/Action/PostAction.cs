using Sia.Domain.ApiModels.Playbooks;
using Sia.Domain.Playbook;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;
using System.Net.Http;

namespace Sia.Playbook.Requests
{
    public class PostActionRequest : AuthenticatedRequest<Action>
    {
        public PostActionRequest(CreateAction createAction, AuthenticatedUserContext userContext) : base(userContext)
        {
            CreateAction = createAction;
        }

        public CreateAction CreateAction { get; }
    }

    public class PostActionHandler : PlaybookProxyHandler<PostActionRequest, Action>
    {
        public PostActionHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(PostActionRequest request) => request.CreateAction;
        protected override HttpMethod Method() => HttpMethod.Post;
        protected override string RelativeUri(PostActionRequest request) => throw new System.NotImplementedException();
    }
}
