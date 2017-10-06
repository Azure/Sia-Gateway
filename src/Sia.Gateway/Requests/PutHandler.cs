using MediatR;
using Sia.Gateway.ServiceRepositories.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class PutHandler<TRepository, TRequest>
        : IAsyncRequestHandler<TRequest>
        where TRequest : IRequest
        where TRepository : IPut<TRequest>
    {
        protected PutHandler(TRepository repository)
        {
            _repository = repository;
        }

        protected TRepository _repository { get; }

        public virtual Task Handle(TRequest request)
            => _repository.PutAsync(request);
    }
}
