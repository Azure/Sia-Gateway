using MediatR;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Extensions.Mediatr;
using Sia.Shared.Requests;

namespace Sia.Gateway.Requests.Playbook
{
    public abstract class PlaybookProxyHandler<TRequest, TResult>
        : ProxyHandler<TRequest, TResult>
        where TRequest : AuthenticatedRequest<TResult>
    {
        public const string PlaybookEndpointName = "Playbook";
        protected PlaybookProxyHandler(HttpClientLookup clientFactory)
            : base(clientFactory, PlaybookEndpointName)
        {
        }
    }

    public abstract class PlaybookProxyHandler<TRequest>
        : ProxyHandler<TRequest>
        where TRequest : AuthenticatedRequest
    {
        public const string PlaybookEndpointName = "Playbook";
        protected PlaybookProxyHandler(HttpClientLookup clientFactory)
            : base(clientFactory, PlaybookEndpointName)
        {
        }
    }
}
