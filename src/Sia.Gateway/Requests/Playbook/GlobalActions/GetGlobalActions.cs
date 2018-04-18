using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sia.Domain.Playbook;
using Sia.Gateway.Initialization.Configuration;
using Sia.Gateway.Requests.Playbook;
using Sia.Core.Authentication;
using Sia.Core.Authentication.Http;
using Sia.Core.Requests;

namespace Sia.Gateway.Requests
{
    public class GetGlobalActionsRequest : AuthenticatedRequest<IEnumerable<Action>>
    {
        public GetGlobalActionsRequest(AuthenticatedUserContext userContext)
            : base(userContext)
        {
        }
    }

    public class GetGlobalActionsShortCircuit : PlaybookShortCircuit<GetGlobalActionsRequest, IEnumerable<Action>>
    {
        public GetGlobalActionsShortCircuit(MicroservicesConfig config) : base(config)
        {

        }

        public override Task<IEnumerable<Action>> GenerateMockAsync(GetGlobalActionsRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<Action>().AsEnumerable());
        }

        public static IServiceCollection RegisterMe(IServiceCollection services)
            => services.AddTransient<IPipelineBehavior<GetGlobalActionsRequest, IEnumerable<Action>>, GetGlobalActionsShortCircuit>();
    }

    public class GetGlobalActionsHandler : PlaybookProxyHandler<GetGlobalActionsRequest, IEnumerable<Action>>
    {
        public GetGlobalActionsHandler(HttpClientLookup clientFactory) : base(clientFactory)
        {
          
        }

        protected override object MessageContent(GetGlobalActionsRequest request)
            => null;

        protected override HttpMethod Method()
            => HttpMethod.Get;

        protected override string RelativeUri(GetGlobalActionsRequest request)
            => "/globalActions/";
    }
}
