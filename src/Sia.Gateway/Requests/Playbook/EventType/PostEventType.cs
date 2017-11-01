using AutoMapper;
using Sia.Domain.ApiModels.Playbooks;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Requests;
using System.Threading.Tasks;
using System.Net.Http;
using Sia.Shared.Authentication.Http;

namespace Sia.Gateway.Requests
{
    public class PostEventTypeRequest : AuthenticatedRequest<Domain.Playbook.EventType>
    {
        public PostEventTypeRequest(CreateEventType createEventType, AuthenticatedUserContext userContext) : base(userContext)
        {
            CreateEventType = createEventType;
        }

        public CreateEventType CreateEventType { get; }
    }

    public class PostEventTypeHandler : PlaybookProxyHandler<PostEventTypeRequest, Domain.Playbook.EventType>
    {
        public PostEventTypeHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(PostEventTypeRequest request)
            => request.CreateEventType;
        protected override HttpMethod Method()
            => HttpMethod.Post;
        protected override string RelativeUri(PostEventTypeRequest request)
            => "/eventTypes/";
    }
}
