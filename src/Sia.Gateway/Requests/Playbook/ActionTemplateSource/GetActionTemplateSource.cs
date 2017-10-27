using Sia.Domain.Playbook;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;
using System.Net.Http;

namespace Sia.Playbook.Requests
{
    public class GetActionTemplateSourceRequest : AuthenticatedRequest<Domain.Playbook.ActionTemplateSource>
    {
        public GetActionTemplateSourceRequest(long actionTemplateSourceId, long actionTemplateId, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            ActionTemplateSourceId = actionTemplateSourceId;
            ActionTemplateId = actionTemplateId;
        }

        public long ActionTemplateSourceId { get; private set; }
        public long ActionTemplateId { get; private set; }
    }

    public class GetActionTemplateSourceHandler : PlaybookProxyHandler<GetActionTemplateSourceRequest, ActionTemplateSource>
    {
        public GetActionTemplateSourceHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(GetActionTemplateSourceRequest request)
            => null;
        protected override HttpMethod Method()
            => HttpMethod.Get;
        protected override string RelativeUri(GetActionTemplateSourceRequest request)
            => $"/actionTemplates/{request.ActionTemplateId}/actionTemplateSources/{request.ActionTemplateSourceId}/";
    }
}
