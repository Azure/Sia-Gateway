using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Requests;
using Sia.Core.Authentication;
using Sia.Core.Controllers;
using Sia.Core.Protocol;
using System.Threading.Tasks;
using System;
using Sia.Core.Validation;
using Sia.Gateway.Links;

namespace Sia.Gateway.Controllers
{
    [Route("[controller]")]
    public class IncidentsController : BaseController
    {
        public IncidentsController(
            IMediator mediator,
            AzureActiveDirectoryAuthenticationInfo authConfig,
            IUrlHelper urlHelper,
            IncidentLinksProvider links) 
            : base(mediator, authConfig, urlHelper)
        {
            Links = links;
        }

        protected IncidentLinksProvider Links { get; }

        [HttpGet("{incidentId}", Name = IncidentRoutes.GetSingle)]
        public async Task<IActionResult> Get(long incidentId)
        {
            var result = await _mediator
                .Send(new GetIncidentRequest(incidentId, AuthContext))
                .ConfigureAwait(continueOnCapturedContext: false);

            var links = Links.CreateLinks(incidentId);

            return OkIfFound(result, links);
        }

        [HttpGet(Name = IncidentRoutes.GetMultiple)]
        public async Task<IActionResult> Get([FromQuery] PaginationMetadata pagination)
        {
            var result = await _mediator
                .Send(new GetIncidentsRequest(pagination, AuthContext))
                .ConfigureAwait(continueOnCapturedContext: false);

            var links = Links.CreatePaginatedLinks(IncidentRoutes.GetSingle, pagination);

            return OkIfFound(result, links);
        }

        [HttpPost(Name = IncidentRoutes.PostSingle)]
        public async Task<IActionResult> Post([FromBody]NewIncident incident)
        {
            var result = await _mediator
                .Send(new PostIncidentRequest(incident, AuthContext))
                .ConfigureAwait(continueOnCapturedContext: false);

            ILinksHeader getLinks(Incident res) =>
                Links.CreateLinks(res.Id);

            Uri getRetrievalRoute(Incident res) =>
                new Uri(_urlHelper.Link(IncidentRoutes.GetSingle, new { incidentId = res.Id }));

            return CreatedIfExists(result, getRetrievalRoute, getLinks);
        }
    }
}
