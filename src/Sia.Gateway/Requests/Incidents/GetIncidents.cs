using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Shared.Authentication;
using Sia.Shared.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class GetIncidentsRequest : AuthenticatedRequest<IEnumerable<Incident>>
    {
        public GetIncidentsRequest(AuthenticatedUserContext userContext)
            : base(userContext)
        {
        }
    }

    public class GetIncidentsHandler
        : IncidentContextHandler<GetIncidentsRequest, IEnumerable<Incident>>
    {
        public GetIncidentsHandler(IncidentContext context)
            :base(context)
        {

        }
        public override async Task<IEnumerable<Incident>> Handle(GetIncidentsRequest request)
        {
            var incidentRecords = await _context.Incidents
                .WithEagerLoading()
                .ProjectTo<Incident>()
                .ToListAsync();
            return incidentRecords;
        }
    }

}
