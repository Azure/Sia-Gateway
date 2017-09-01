using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sia.Connectors.Tickets;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.ServiceRepositories
{
    public interface IIncidentRepository
    {
        Task<Incident> GetIncidentAsync(long id, AuthenticatedUserContext userContext);
        Task<IEnumerable<Incident>> GetIncidentsAsync(AuthenticatedUserContext userContext);
        Task<Incident> PostIncidentAsync(NewIncident incident, AuthenticatedUserContext userContext);
        Task<IEnumerable<Incident>> GetIncidentsByTicketAsync(string ticketId, AuthenticatedUserContext userContext);
    }
    public class IncidentRepository<TTicket> : IIncidentRepository
    {
        private readonly IncidentContext _context;
        private readonly Connector<TTicket> _connector;

        public IncidentRepository(IncidentContext context, Connector<TTicket> connector)
        {
            _context = context;
            _connector = connector;
        }

        public async Task<Incident> GetIncidentAsync(long id, AuthenticatedUserContext userContext)
        {
            var incidentRecord = await _context.Incidents.WithEagerLoading().FirstOrDefaultAsync(cr => cr.Id == id);
            if (incidentRecord == null) throw new KeyNotFoundException();

            var ticket = await _connector.Client.GetAsync(incidentRecord.PrimaryTicket.OriginId);

            return _connector.Converter.AssembleIncident(incidentRecord, ticket);
        }

        public async Task<IEnumerable<Incident>> GetIncidentsAsync(AuthenticatedUserContext userContext)
        {
            var incidentRecords = await _context.Incidents.WithEagerLoading().ProjectTo<Incident>().ToListAsync();
            return incidentRecords;
        }

        public async Task<IEnumerable<Incident>> GetIncidentsByTicketAsync(string ticketId, AuthenticatedUserContext userContext)
        {
            //No idea why this doesn't work as a single linq statement
            var incidentRecords = _context.Incidents
                .WithEagerLoading();
            var filteredIncidentRecords1 = incidentRecords
                .Where(incident => incident.Tickets.Any(inc => inc.OriginId == ticketId));
            var filteredIncidentRecords2 = incidentRecords
                .Where(incident => incident.PrimaryTicket.OriginId == ticketId);
            var filteredIncidentRecords = filteredIncidentRecords1.Union(filteredIncidentRecords2);
            var localIncidentRecords = await filteredIncidentRecords.ToListAsync();
            var projectedIncidentRecords = localIncidentRecords.AsQueryable().ProjectTo<Incident>();
            return projectedIncidentRecords;
        }

        public async Task<Incident> PostIncidentAsync(NewIncident incident, AuthenticatedUserContext userContext)
        {
            if (incident == null) throw new ArgumentNullException(nameof(incident));
            if (incident?.PrimaryTicket?.OriginId == null) throw new ConflictException("Please provide a primary incident with a valid originId");

            var dataIncident = Mapper.Map<Data.Incidents.Models.Incident>(incident);

            var result = _context.Incidents.Add(dataIncident);
            await _context.SaveChangesAsync();

            return Mapper.Map<Incident>(dataIncident);
        }
        
    }
}
