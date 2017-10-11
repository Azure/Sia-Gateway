using MediatR;
using Sia.Domain;
using Sia.Gateway.Authentication;
using Sia.Gateway.ServiceRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests
{
    public class GetIncidentsRequest : AuthenticatedRequest, IRequest<IEnumerable<Incident>>
    {
        public GetIncidentsRequest(AuthenticatedUserContext userContext) : base(userContext)
        {
        }
    }
}
