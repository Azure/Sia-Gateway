using System.Collections.Generic;
using System.Net.Http;
using Sia.Domain.Playbook;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Requests;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Sia.Shared.Extensions.Mediatr;

namespace Sia.Gateway.Requests
{
    public class GetEventTypesRequest : AuthenticatedRequest<IEnumerable<EventType>>
    {
        public GetEventTypesRequest(AuthenticatedUserContext userContext)
            : base(userContext)
        {
        }
    }

    public class GetEventTypesShortCircuit : PlaybookShortCircuit<GetEventTypesRequest, IEnumerable<EventType>>
    {
        public GetEventTypesShortCircuit(IConfigurationRoot config) : base(config)
        {

        }

        public override Task<IEnumerable<EventType>> GenerateMockAsync(GetEventTypesRequest request, CancellationToken cancellationToken)
        {
            var eventType = new EventType()
            {
                Id = 100000000,
                Name = "This is a mock",
                Data = new MockEventTypeData()
            };
            var result = new List<EventType>
            {
                eventType
            };
            return Task.FromResult(result.AsEnumerable());
        }
    }

    public class GetEventTypesHandler : PlaybookProxyHandler<GetEventTypesRequest, IEnumerable<EventType>>
    {
        public GetEventTypesHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(GetEventTypesRequest request)
            => null;

        protected override HttpMethod Method()
            => HttpMethod.Get;

        protected override string RelativeUri(GetEventTypesRequest request)
            => "/eventTypes/";
    }
}
