using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sia.Domain.ApiModels;
using Sia.Core.Authentication;
using Sia.Gateway.Requests;
using System.Threading.Tasks;
using Sia.Core.Controllers;
using System;

namespace Sia.Gateway.Controllers
{
    [Route("incidents/{incidentId}/engagements")]
    public class EngagementsController : BaseController
    {
        private const string notFoundMessage = "Incident or engagement not found";

        public EngagementsController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, IUrlHelper urlHelper) 
            : base(mediator, authConfig, urlHelper)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]long incidentId, [FromRoute]long id)
        {
            var result = await _mediator
                .Send(new GetEngagementRequest(incidentId, id, AuthContext))
                .ConfigureAwait(continueOnCapturedContext: false);
            return OkIfFound(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Post([FromRoute]long incidentId, [FromBody]NewEngagement newEngagement)
        {
            var result = await _mediator
                .Send(new PostEngagementRequest(incidentId, newEngagement, AuthContext))
                .ConfigureAwait(continueOnCapturedContext: false);
            if (result == null)
            {
                return NotFound(notFoundMessage);
            }
            return Created(new Uri($"incidents/{result.IncidentId}/engagements/{result.Id}"), result);
        }

        [HttpPut("{engagementId}")]
        public async Task<IActionResult> Put([FromRoute]long incidentId, [FromRoute]long engagementId, [FromBody]UpdateEngagement updatedEngagement)
        {
            await _mediator
                .Send(new PutEngagementRequest(incidentId, engagementId, updatedEngagement, AuthContext))
                .ConfigureAwait(continueOnCapturedContext: false);
            return Ok();
        }
    }
}
