using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sia.Connectors.Tickets;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.Requests;
using Sia.Gateway.ServiceRepositories.Operations;
using Sia.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.ServiceRepositories
{
    public interface IIncidentRepository
        : IGet<GetIncidentRequest, Incident>,
        IGetMany<GetIncidentsRequest, Incident>,
        IGetMany<GetIncidentsByTicketRequest, Incident>,
        IPost<PostIncidentRequest, Incident>
    {
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

        public async Task<Incident> GetAsync(GetIncidentRequest getIncident)
        {
            var incidentRecord = await _context.Incidents.WithEagerLoading().FirstOrDefaultAsync(cr => cr.Id == getIncident.Id);
            if (incidentRecord == null) throw new KeyNotFoundException();

            var ticket = await _connector.Client.GetAsync(incidentRecord.Tickets.FirstOrDefault(t => t.IsPrimary).OriginId);

            return _connector.Converter.AssembleIncident(incidentRecord, ticket);
        }

        public async Task<IEnumerable<Incident>> GetManyAsync(GetIncidentsRequest request)
        {
            var incidentRecords = await _context.Incidents
                .WithEagerLoading()
                .ProjectTo<Incident>()
                .ToListAsync();
            return incidentRecords;
        }

        public async Task<IEnumerable<Incident>> GetManyAsync(GetIncidentsByTicketRequest request)
        {
            var incidentRecords = await _context.Incidents
                .WithEagerLoading()
                .Where(incident => incident.Tickets.Any(inc => inc.OriginId == request.TicketId))
                .ProjectTo<Incident>().ToListAsync();

            return incidentRecords;
        }

        public async Task<Incident> PostAsync(PostIncidentRequest request)
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
