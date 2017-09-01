using AutoMapper;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.TestDoubles
{
    public class StubEventRepository : IEventRepository
    {
        private List<Event> _events;

        public StubEventRepository(Event ev)
            : this(new List<Event>() { ev }) { }
        public StubEventRepository(ICollection<Event> events)
        {
            _events = events.ToList();
            StatusCodeToRespondWith = HttpStatusCode.OK;
            IsSuccessStatusCodeToRespondWith = true;
            ContentToRespondWith = "You weren't going to use this anyway";
        }

        public HttpStatusCode StatusCodeToRespondWith { get; set; }
        public bool IsSuccessStatusCodeToRespondWith { get; set; }
        public string ContentToRespondWith { get; set; }

        public Task<Event> GetEvent(long incidentId, long id, AuthenticatedUserContext userContext)
        {
            return Task.FromResult(_events.First(ev => ev.Id == id && ev.IncidentId == incidentId));
        }
        
        public Task<Event> PostEvent(long incidentId, NewEvent newEvent, AuthenticatedUserContext userContext)
        {
            return Task.FromResult(Mapper.Map(newEvent, new Event()));
        }
    }
}
