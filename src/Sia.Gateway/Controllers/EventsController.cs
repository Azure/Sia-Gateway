using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.Protocol;
using Sia.Gateway.Requests;
using Sia.Gateway.Requests.Events;
using System.Threading.Tasks;

namespace Sia.Gateway.Controllers
{
    [Route("incidents/{incidentId}/events")]
    public class EventsController : BaseController
    {
        private const string notFoundMessage = "Incident or event not found";

        public EventsController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig)
            : base(mediator, authConfig)
        {
            
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromRoute]long incidentId, [FromQuery]PaginationMetadata pagination)
        {
            var result = await _mediator.Send(new GetEventsRequest(incidentId, pagination, _authContext));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]long incidentId, [FromRoute]long id)
        {
            var result = await _mediator.Send(new GetEventRequest(incidentId, id, _authContext));
            if (result == null)
            {
                return NotFound(notFoundMessage);
            }
            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Post([FromRoute]long incidentId, [FromBody]NewEvent newEvent)
        {
            var result = await _mediator.Send(new PostEventRequest(incidentId, newEvent, _authContext));
            if (result == null)
            {
                return NotFound(notFoundMessage);
            }
            return Created($"api/incidents/{result.IncidentId}/events/{result.Id}", result);
        }
    }
}
