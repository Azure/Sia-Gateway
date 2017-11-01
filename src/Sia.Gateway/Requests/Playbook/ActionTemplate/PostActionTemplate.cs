using Sia.Shared.Authentication;
using Sia.Domain.ApiModels.Playbooks;
using Sia.Shared.Requests;
using Sia.Gateway.Requests.Playbook;
using System.Net.Http;
using Sia.Shared.Authentication.Http;

namespace Sia.Gateway.Requests
{
    public class PostActionTemplateRequest : AuthenticatedRequest<Domain.Playbook.ActionTemplate>
    {
        public PostActionTemplateRequest(CreateActionTemplate createActionTemplate, AuthenticatedUserContext userContext) 
            : base(userContext)
        {
            CreateActionTemplate = createActionTemplate;
        }

        public CreateActionTemplate CreateActionTemplate { get; }
    }

    public class PostActionTemplateHandler : PlaybookProxyHandler<PostActionTemplateRequest, Domain.Playbook.ActionTemplate>
    {
        public PostActionTemplateHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(PostActionTemplateRequest request)
            => request.CreateActionTemplate;
        protected override HttpMethod Method()
            => HttpMethod.Post;
        protected override string RelativeUri(PostActionTemplateRequest request)
            => $"/actionTemplates/";
    }
}
