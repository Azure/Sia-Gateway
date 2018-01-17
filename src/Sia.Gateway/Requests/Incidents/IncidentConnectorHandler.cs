﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sia.Data.Incidents;
using Sia.Connectors.Tickets;
using MediatR;
using Sia.Domain;
using Microsoft.Extensions.Logging;

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

        protected void AttachTickets(Incident incident)
        {
     
            if (_connector.Logger.IsEnabled(LogLevel.Debug)) {
                _connector.Logger.LogDebug("Incident Data For Ticket", incident);
            }
            _connector.AppendData(incident.Tickets);
        }

        protected void AttachTickets(List<Incident> incidents)
            => incidents.ForEach(inc => AttachTickets(inc));
               
    }
}
