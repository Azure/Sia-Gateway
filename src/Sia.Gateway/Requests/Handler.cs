using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public abstract class Handler<TRequest, TReturn>
        : IAsyncRequestHandler<TRequest, TReturn>
        where TRequest : IRequest<TReturn>
    {
        public abstract Task<TReturn> Handle(TRequest request);
    }

    public abstract class Handler<TRequest>
        : IAsyncRequestHandler<TRequest>
        where TRequest : IRequest
    {
        public abstract Task Handle(TRequest request);
    }
}
