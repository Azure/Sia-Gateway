using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sia.Data.Incidents;
using Sia.Connectors.Tickets;
using MediatR;
using Sia.Domain;

namespace Sia.Gateway.Requests
{
    public abstract class IncidentConnectorHandler<TRequest, TResult>
        : IncidentContextHandler<TRequest, TResult>
        where TRequest : IRequest<TResult>
    {
        protected readonly Connector _connector;
        protected IncidentConnectorHandler(
            IncidentContext context,
            Connector connector
        ) : base(context)
        {
            _connector = connector;
        }

        protected async Task AttachTickets(Incident incident)
            => incident.Tickets = (await _connector
                    .GetData(incident.Tickets.AsEnumerable())
                ).ToList();

        protected void AttachTickets(List<Incident> incidents)
            => Task.WaitAll(
                incidents
                .Select(inc => AttachTickets(inc))
                .ToArray()
            );
    }
}
