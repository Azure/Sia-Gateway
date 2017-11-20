using MediatR;
using Sia.Data.Incidents;
using Sia.Gateway.Requests;
using Sia.Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public abstract class IncidentContextHandler<TRequest, TResponse>
        : DatabaseOperationHandler<IncidentContext, TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        protected IncidentContextHandler(IncidentContext context) : base(context)
        {
        }
    }

    public abstract class IncidentContextHandler<TRequest>
    : DatabaseOperationHandler<IncidentContext, TRequest>
    where TRequest : IRequest
    {
        protected IncidentContextHandler(IncidentContext context) : base(context)
        {
        }
    }
}
