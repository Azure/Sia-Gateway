using MediatR;
using Sia.Gateway.ServiceRepositories.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public abstract class GetManyHandler<TRepository, TRequest, TReturn>
        : Handler<TRequest, IEnumerable<TReturn>>
        where TRequest : IRequest<IEnumerable<TReturn>>
        where TRepository : IGetMany<TRequest, TReturn>
    {
        protected GetManyHandler(TRepository repository)
        {
            _repository = repository;
        }

        protected TRepository _repository { get; }

        public override Task<IEnumerable<TReturn>> Handle(TRequest request)
            => _repository.GetManyAsync(request);
    }
}
