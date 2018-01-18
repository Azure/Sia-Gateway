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

namespace Sia.Gateway.Controllers
{
    [Route("incidents/{incidentId}/events")]
    public class EventsController : BaseController
    {
        private const string notFoundMessage = "Incident or event not found";
        private readonly HubConnectionBuilder _hubConnectionBuilder;
        private readonly OperationLinks _operationLinks;
        private readonly RelationLinks _relationLinks;

        public EventsController(IMediator mediator,
            AzureActiveDirectoryAuthenticationInfo authConfig,
            HubConnectionBuilder hubConnectionBuilder,
            IUrlHelper urlHelper)
            : base(mediator, authConfig, urlHelper)
        {
            _hubConnectionBuilder = hubConnectionBuilder;
            _operationLinks = new OperationLinks()
            {
                Single = new SingleOperationLinks()
                {
                    Get = _urlHelper.Action(GetSingleRouteName),
                    Post = _urlHelper.Action(PostSingleRouteName)
                },
                Multiple = new MultipleOperationLinks()
                {
                    Get = _urlHelper.Action(GetMultipleRouteName)
                }
            };
            _relationLinks = new RelationLinks()
            {
                Parent = new RelatedParentLinks()
                {
                    Incident = _urlHelper.Action(IncidentsController.GetSingleRouteName, nameof(IncidentsController))
                }
            };
        }

        public const string GetMultipleRouteName = nameof(GetEvents);
        [HttpGet(Name = GetMultipleRouteName)]
        public async Task<IActionResult> GetEvents([FromRoute]long incidentId,
            [FromQuery]PaginationMetadata pagination,
            [FromQuery]EventFilters filter)
        {
            var result = await _mediator.Send(new GetEventsRequest(incidentId, pagination, filter, _authContext));
            Response.Headers.AddLinksHeader(new FilteredLinksHeader(filter, pagination, _urlHelper, GetMultipleRouteName, _operationLinks, _relationLinks));
            return Ok(result);
        }

        public const string GetSingleRouteName = "GetEvent";
        [HttpGet("{id}", Name = GetSingleRouteName)]
        public async Task<IActionResult> Get([FromRoute]long incidentId, [FromRoute]long id)
        {
            var result = await _mediator.Send(new GetEventRequest(incidentId, id, _authContext));
            Response.Headers.AddLinksHeader(new LinksHeader(null, _urlHelper, GetSingleRouteName, _operationLinks, _relationLinks));
            if (result == null)
            {
                return NotFound(notFoundMessage);
            }
                
            return Ok(result);
        }

        public const string PostSingleRouteName = "PostEvent";
        [HttpPost(Name = PostSingleRouteName)]
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
