using MediatR;
using Sia.Gateway.ServiceRepositories.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public abstract class GetHandler<TRepository, TRequest, TReturn>
        : Handler<TRequest, TReturn>
        where TRequest : IRequest<TReturn>
        where TRepository : IGet<TRequest, TReturn>
    {
        protected GetHandler(TRepository repository)
        {
            _repository = repository;
        }

        protected TRepository _repository { get; }

        public override Task<TReturn> Handle(TRequest request)
            => _repository.GetAsync(request);
    }
}
