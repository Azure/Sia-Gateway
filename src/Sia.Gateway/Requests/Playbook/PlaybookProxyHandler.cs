using MediatR;
using Sia.Core.Authentication.Http;
using Sia.Core.Extensions.Mediatr;
using Sia.Core.Requests;

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
