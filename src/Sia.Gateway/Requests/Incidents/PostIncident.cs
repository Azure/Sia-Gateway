using AutoMapper;
using MediatR;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Shared.Authentication;
using Sia.Shared.Exceptions;
using Sia.Shared.Requests;
using System;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class PostIncidentRequest : AuthenticatedRequest<Incident>
    {
        public PostIncidentRequest(NewIncident incident, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            Incident = incident;
        }

        public NewIncident Incident { get; private set; }
    }

    public class PostIncidentHandler
    : IncidentContextHandler<PostIncidentRequest, Incident>
    {
        public PostIncidentHandler(IncidentContext context)
            :base(context)
        {

        }
        public override async Task<Incident> Handle(PostIncidentRequest request)
        {
            if (request.Incident == null) throw new ArgumentNullException(nameof(request.Incident));
            if (request.Incident?.PrimaryTicket?.OriginId == null) throw new ConflictException("Please provide a primary incident with a valid originId");

            var dataIncident = Mapper.Map<Data.Incidents.Models.Incident>(request.Incident);

            var result = _context.Incidents.Add(dataIncident);
            await _context.SaveChangesAsync();

            return Mapper.Map<Incident>(dataIncident);
        }
    }
}
