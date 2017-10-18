using AutoMapper;
using MediatR;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using Sia.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class PostIncidentRequest : AuthenticatedRequest, IRequest<Incident>
    {
        public PostIncidentRequest(NewIncident incident, AuthenticatedUserContext userContext)
            :base(userContext)
        {
            Incident = incident;
        }

        public NewIncident Incident { get; private set; }
    }

    public class PostIncidentHandler
    : IAsyncRequestHandler<PostIncidentRequest, Incident>
    {
        private readonly IncidentContext _context;
        public PostIncidentHandler(IncidentContext context)
        {
            _context = context;
        }
        public async Task<Incident> Handle(PostIncidentRequest request)
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
