using Sia.Domain.Playbook;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;
using System.Net.Http;

namespace Sia.Playbook.Requests
{
    public class GetActionTemplateRequest : AuthenticatedRequest<ActionTemplate>
    {
        public GetActionTemplateRequest(long actionTemplateId, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            ActionTemplateId = actionTemplateId;
        }

        public long ActionTemplateId { get; private set; }
    }

    public class GetActionTemplateHandler : PlaybookProxyHandler<GetActionTemplateRequest, Domain.Playbook.ActionTemplate>
    {
        public GetActionTemplateHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(GetActionTemplateRequest request)
            => null;
        protected override HttpMethod Method()
            => HttpMethod.Get;
        protected override string RelativeUri(GetActionTemplateRequest request)
            => $"/actionTemplates/{request.ActionTemplateId}/";
    }
}
