using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class GetIncidentsRequest : AuthenticatedRequest, IRequest<IEnumerable<Incident>>
    {
        public GetIncidentsRequest(AuthenticatedUserContext userContext)
            : base(userContext)
        {
        }
    }

    public class GetIncidentsHandler
        : IAsyncRequestHandler<GetIncidentsRequest, IEnumerable<Incident>>
    {
        private readonly IncidentContext _context;
        public GetIncidentsHandler(IncidentContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Incident>> Handle(GetIncidentsRequest request)
        {
            var incidentRecords = await _context.Incidents
                .WithEagerLoading()
                .ProjectTo<Incident>()
                .ToListAsync();
            return incidentRecords;
        }
    }

}
