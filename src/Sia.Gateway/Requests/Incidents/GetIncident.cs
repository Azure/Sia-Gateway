using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Connectors.Tickets;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Shared.Authentication;
using Sia.Shared.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class GetIncidentRequest : AuthenticatedRequest<Incident>
    {
        public GetIncidentRequest(long id, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            Id = id;
        }
        public long Id { get; }
    }

    public class GetIncidentHandler<TTicket> : IGetIncidentHandler
    {
        private readonly IncidentContext _context;
        private readonly Connector<TTicket> _connector;
        public GetIncidentHandler(IncidentContext context, Connector<TTicket> connector)
        {
            _context = context;
            _connector = connector;
        }
        public async Task<Incident> Handle(GetIncidentRequest getIncident)
        {
            var incidentRecord = await _context.Incidents
                                        .WithEagerLoading()
                                        .FirstOrDefaultAsync(cr => cr.Id == getIncident.Id);
            if (incidentRecord == null) throw new KeyNotFoundException();

            var remoteId = incidentRecord
                                .Tickets
                                .FirstOrDefault(t => t.IsPrimary)
                                .OriginId;

            var ticket = await _connector.Client.GetAsync(remoteId);

            return _connector
                    .Converter
                    .AssembleIncident(incidentRecord, ticket);
        }
    }

    public interface IGetIncidentHandler
    {
        Task<Incident> Handle(GetIncidentRequest getIncident);
    }

    //Why does this exist?
    //Purely because I haven't been able to get Mediatr to work with generics
    public class GetIncidentHandlerWrapper : IAsyncRequestHandler<GetIncidentRequest, Incident>
    {
        private readonly IGetIncidentHandler _actualHandler;

        public GetIncidentHandlerWrapper(IGetIncidentHandler actualHandler)
        {
            _actualHandler = actualHandler;
        }

        public Task<Incident> Handle(GetIncidentRequest message)
            => _actualHandler.Handle(message);
    }
}
