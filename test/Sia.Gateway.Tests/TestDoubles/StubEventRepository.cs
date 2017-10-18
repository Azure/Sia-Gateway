using AutoMapper;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.Requests;
using Sia.Gateway.ServiceRepositories;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Sia.Gateway.Protocol;
using Sia.Gateway.Requests.Events;

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

        public Task<Event> Handle(GetEventRequest request) 
            => Task.FromResult(_events.First(ev => ev.Id == request.Id && ev.IncidentId == request.IncidentId));

        public Task<IEnumerable<Event>> Handle(GetEventsRequest request)
            => Task.FromResult(_events.AsEnumerable());

        public Task<Event> Handle(PostEventRequest request)
            => Task.FromResult(Mapper.Map(request.NewEvent, new Event()));
    }
}
