using Sia.Shared.Authentication;
using Sia.Shared.Requests;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication.Http;
using System.Net.Http;

namespace Sia.Playbook.Requests
{
    public class AssociateActionWithEventTypeRequest : AuthenticatedRequest
    {
        public AssociateActionWithEventTypeRequest(long actionId, long eventTypeId, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            ActionId = actionId;
            EventTypeId = eventTypeId;
        }

        public long ActionId { get; }
        public long EventTypeId { get; }
    }

    public class AssociateActionWithEventTypeHandler
        : PlaybookProxyHandler<AssociateActionWithEventTypeRequest>
    {
        public AssociateActionWithEventTypeHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(AssociateActionWithEventTypeRequest request) 
            => null;
        protected override HttpMethod Method() => HttpMethod.Put;
        protected override string RelativeUri(AssociateActionWithEventTypeRequest request) 
            => $"/actions/{request.ActionId}/eventTypes/{request.EventTypeId}";
    }
}
