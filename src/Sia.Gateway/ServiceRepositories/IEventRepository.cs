using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.ServiceRepositories
{
    public interface IEventRepository
    {
        Task<Event> GetEvent(long incidentId, long id, AuthenticatedUserContext userContext);
        Task<Event> PostEvent(long incidentId, NewEvent newEvent, AuthenticatedUserContext userContext);
    }

    public class EventRepository : IEventRepository
    {

        private readonly IncidentContext _context;

        public EventRepository(IncidentContext context)
        {
            _context = context;
        }

        public async Task<Event> GetEvent(long incidentId, long id, AuthenticatedUserContext userContext)
        {
            var eventRecord = await _context.Events.FirstOrDefaultAsync(ev => ev.IncidentId == incidentId && ev.Id == id);
            if (eventRecord == null) throw new KeyNotFoundException();

            return Mapper.Map<Event>(eventRecord);
        }

        public async Task<Event> PostEvent(long incidentId, NewEvent newEvent, AuthenticatedUserContext userContext)
        {
            if (newEvent == null) throw new ArgumentNullException(nameof(newEvent));

            var dataCrisis = await _context.Incidents
               .Include(cr => cr.Events)
               .FirstOrDefaultAsync(x => x.Id == incidentId);
            if (dataCrisis == null) throw new KeyNotFoundException();

            var dataEvent = Mapper.Map<Data.Incidents.Models.Event>(newEvent);

            dataCrisis.Events.Add(dataEvent);
            await _context.SaveChangesAsync();

            return Mapper.Map<Event>(dataEvent);
        }        
    }
}
