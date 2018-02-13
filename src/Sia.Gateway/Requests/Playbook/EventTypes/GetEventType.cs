using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sia.Domain.Playbook;
using Sia.Gateway.Initialization.Configuration;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Extensions.Mediatr;
using Sia.Shared.Requests;

namespace Sia.Gateway.Requests
{
    public class GetEventTypeRequest : AuthenticatedRequest<EventType>
    {
        public GetEventTypeRequest(long id, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            EventTypeId = id;
        }

        public long EventTypeId { get; private set; }
    }

    public class GetEventTypeShortCircuit : PlaybookShortCircuit<GetEventTypeRequest, EventType>
    {
        public GetEventTypeShortCircuit(MicroservicesConfig config) : base(config)
        {

        }

        public override Task<EventType> GenerateMockAsync(GetEventTypeRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new EventType()
            {
                Id = request.EventTypeId,
                Name = "This is a mock",
                Data = new MockEventTypeData()
            });
        }

        public static IServiceCollection RegisterMe(IServiceCollection services)
            => services.AddTransient<IPipelineBehavior<GetEventTypeRequest, EventType>, GetEventTypeShortCircuit>();
    }

    public class GetEventTypeHandler : PlaybookProxyHandler<GetEventTypeRequest, EventType>
    {
        public GetEventTypeHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
        }

        protected override object MessageContent(GetEventTypeRequest request)
            => null;
        protected override HttpMethod Method()
            => HttpMethod.Get;
        protected override string RelativeUri(GetEventTypeRequest request)
            => $"/eventTypes/{request.EventTypeId}";
    }
}
