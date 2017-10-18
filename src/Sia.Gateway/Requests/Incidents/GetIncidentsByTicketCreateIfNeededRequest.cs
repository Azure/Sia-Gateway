using MediatR;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class GetIncidentsByTicketCreateIfNeededRequest : AuthenticatedRequest, IRequest<IEnumerable<Incident>>
    {
        public GetIncidentsByTicketCreateIfNeededRequest(string ticketId, AuthenticatedUserContext userContext)
            : base(userContext)
        {
            TicketId = ticketId;
        }

        public string TicketId { get; private set; }

    }

    public class GetIncidentsByTicketCreateIfNeededRequestHandler : IAsyncRequestHandler<GetIncidentsByTicketCreateIfNeededRequest, IEnumerable<Incident>>
    {
        private IIncidentRepository _incidentRepository;

        public GetIncidentsByTicketCreateIfNeededRequestHandler(IIncidentRepository incidentClient)
        {
            _incidentRepository = incidentClient;
        }
        public async Task<IEnumerable<Incident>> Handle(GetIncidentsByTicketCreateIfNeededRequest message)
        {
            var incidents = await _incidentRepository.GetIncidentsByTicketAsync(message.TicketId, message.UserContext);
            if (incidents != null)
            {
                return incidents;
            }

            var primaryTicket = new Ticket
            {
                TicketingSystemId = 1,
                OriginId = message.TicketId
            };
            var newIncident = new NewIncident
            {
                PrimaryTicket = primaryTicket,
                Tickets = new List<Ticket> {
                        primaryTicket
                    }
            };

            var createdIncident = await _incidentRepository.PostIncidentAsync(newIncident, message.UserContext);
            return new List<Incident> { createdIncident };
        }
    }
}
