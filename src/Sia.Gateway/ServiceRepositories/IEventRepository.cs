using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.Protocol;
using Sia.Gateway.Requests;
using Sia.Gateway.Requests.Events;
using Sia.Gateway.ServiceRepositories.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.ServiceRepositories
{
    public interface IEventRepository
        : IGet<GetEventRequest, Event>,
        IPost<PostEventRequest, Event>,
        IGetMany<GetEventsRequest, Event>
    {
    }

    public class EventRepository : IEventRepository
    {

        private readonly IncidentContext _context;

        public EventRepository(IncidentContext context)
        {
            _context = context;
        }

        public async Task<Event> GetAsync(GetEventRequest request)
        {
            var eventRecord = await _context.Events.FirstOrDefaultAsync(ev => ev.IncidentId == request.IncidentId && ev.Id == request.Id);
            if (eventRecord == null) throw new KeyNotFoundException();

            return Mapper.Map<Event>(eventRecord);
        }

        public async Task<IEnumerable<Event>> GetManyAsync(GetEventsRequest request)
        {
            return await _context.Events
                .Where(ev => ev.IncidentId == request.IncidentId)
                .WithPagination(request.Pagination)
                .ProjectTo<Event>()
                .ToListAsync();
        }

        public async Task<Event> PostAsync(PostEventRequest request)
        {
            if (request.NewEvent == null) throw new ArgumentNullException(nameof(request.NewEvent));

            var dataCrisis = await _context.Incidents
               .Include(cr => cr.Events)
               .FirstOrDefaultAsync(x => x.Id == request.IncidentId);
            if (dataCrisis == null) throw new KeyNotFoundException();

            var dataEvent = Mapper.Map<Data.Incidents.Models.Event>(request.NewEvent);

            dataCrisis.Events.Add(dataEvent);
            await _context.SaveChangesAsync();

            return Mapper.Map<Event>(dataEvent);
        }        
    }
}
