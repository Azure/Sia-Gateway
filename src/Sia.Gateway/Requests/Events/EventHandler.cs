using MediatR;
using Sia.Gateway.ServiceRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests.Events
{
    public abstract class EventHandler<TRequest, TReturn> : IAsyncRequestHandler<TRequest, TReturn>
        where TRequest : IRequest<TReturn>
    {
        protected EventHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        protected IEventRepository _eventRepository;

        public abstract Task<TReturn> Handle(TRequest request);
    }
}
