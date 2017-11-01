using Sia.Domain.ApiModels.Playbooks;
using Sia.Domain.Playbook;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;
using System.Net.Http;

namespace Sia.Gateway.Requests
{
    public class PostActionTemplateSourceRequest : AuthenticatedRequest<Domain.Playbook.ActionTemplateSource>
    {
        public PostActionTemplateSourceRequest(long actionTemplateId, CreateActionTemplateSource createActionTemplateSource, AuthenticatedUserContext userContext) : base(userContext)
        {
            CreateActionTemplateSource = createActionTemplateSource;
            ActionTemplateId = actionTemplateId;
        }

        public CreateActionTemplateSource CreateActionTemplateSource { get; }
        public long ActionTemplateId { get; }
    }

    public class PostActionTemplateSourceHandler : PlaybookProxyHandler<PostActionTemplateSourceRequest, ActionTemplateSource>
    {
        public PostActionTemplateSourceHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(PostActionTemplateSourceRequest request)
            => request.CreateActionTemplateSource;
        protected override HttpMethod Method()
            => HttpMethod.Post;
        protected override string RelativeUri(PostActionTemplateSourceRequest request)
            => $"/actionTemplates/{request.ActionTemplateId}/actionTemplateSources/";
    }
}
