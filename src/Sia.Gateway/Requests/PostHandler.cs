using MediatR;
using Sia.Gateway.ServiceRepositories.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public abstract class PostHandler<TRepository, TRequest, TReturn>
        : IAsyncRequestHandler<TRequest, TReturn>
        where TRequest : IRequest<TReturn>
        where TRepository : IPost<TRequest, TReturn>
    {
        protected PostHandler(TRepository repository)
        {
            _repository = repository;
        }

        protected TRepository _repository { get; }

        public virtual Task<TReturn> Handle(TRequest request)
            => _repository.PostAsync(request);
    }
}
