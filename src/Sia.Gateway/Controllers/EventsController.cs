using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Sia.Domain.ApiModels;
using Sia.Shared.Authentication;
using Sia.Shared.Protocol;
using Sia.Gateway.Hubs;
using Sia.Gateway.Requests;
using Sia.Gateway.Requests.Events;
using System.Threading.Tasks;
using Sia.Shared.Controllers;
using Sia.Shared.Data;
using Sia.Data.Incidents.Filters;
using Sia.Data.Incidents.Models;
using Sia.Shared.Protocol.Pagination;
using Sia.Gateway.Protocol.Pagination;

namespace Sia.Gateway.Controllers
{
    [Route("incidents/{incidentId}/events")]
    public class EventsController : BaseController
    {
        private const string notFoundMessage = "Incident or event not found";
        private readonly HubConnectionBuilder _hubConnectionBuilder;

        public EventsController(IMediator mediator,
            AzureActiveDirectoryAuthenticationInfo authConfig,
            HubConnectionBuilder hubConnectionBuilder,
            IUrlHelper urlHelper)
            : base(mediator, authConfig, urlHelper)
        {
            _hubConnectionBuilder = hubConnectionBuilder;
        }

        [HttpGet(Name = nameof(GetEvents))]
        public async Task<IActionResult> GetEvents([FromRoute]long incidentId,
            [FromQuery]EventTimelinePaginationRequest pagination,
            [FromQuery]EventFilters filter)
        {
            var result = await _mediator.Send(new GetEventsRequest(incidentId, pagination, filter, _authContext));
            Response.Headers.AddPagination(new FilteredLinksHeader(filter, result, _urlHelper, nameof(GetEvents)));
            return Ok(result.QueryResult);
        }

        [HttpGet("{id}", Name = "GetEvent")]
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
            await SendEventToSubscribers(result);
            return Created($"api/incidents/{result.IncidentId}/events/{result.Id}", result);
        }

        private async Task SendEventToSubscribers(Domain.Event result)
        {
            var eventHubConnection = _hubConnectionBuilder
                    .WithUrl($"{Request.Scheme}://{Request.Host}/{EventsHub.HubPath}")
                    .Build();
            await eventHubConnection.StartAsync();
            await eventHubConnection.SendAsync("Send", result);
            await eventHubConnection.DisposeAsync();
        }
    }
}
