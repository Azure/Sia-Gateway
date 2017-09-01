using AutoMapper;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.TestDoubles
{
    public class StubIncidentRepository
        : IIncidentRepository

    {
        private IMapper _mapper;

        public StubIncidentRepository(Incident incident, IMapper mapper)
            : this(new List<Incident>() { incident }, mapper) { }
        public StubIncidentRepository(ICollection<Incident> incidents, IMapper mapper)
        {
            _mapper = mapper;
            _incidents = incidents.ToList();
            StatusCodeToRespondWith = HttpStatusCode.OK;
            IsSuccessStatusCodeToRespondWith = true;
            ContentToRespondWith = "You weren't going to use this anyway";
        }

        public HttpStatusCode StatusCodeToRespondWith { get; set; }

        public bool IsSuccessStatusCodeToRespondWith { get; set; }

        public string ContentToRespondWith { get; set; }

        List<Incident> _incidents { get; set; }


        public Task<Incident> GetIncidentAsync(long id, AuthenticatedUserContext userContext)
        {
            return Task.FromResult(_incidents.First(cr => cr.Id == id));
        }

        public Task<IEnumerable<Incident>> GetIncidentsAsync(AuthenticatedUserContext userContext)
        {
            return Task.FromResult(_incidents.AsEnumerable());
        }

        public Task<IEnumerable<Incident>> GetIncidentsByTicketAsync(string ticketId, AuthenticatedUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public Task<Incident> PostIncidentAsync(NewIncident incident, AuthenticatedUserContext userContext)
        {
            return Task.FromResult(_mapper.Map<NewIncident, Incident>(incident));
        }
    }
}
