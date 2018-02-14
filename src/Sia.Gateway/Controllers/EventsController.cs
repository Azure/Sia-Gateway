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
using Sia.Shared.Exceptions;

namespace Sia.Gateway.Controllers
{
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

        public LinksHeader CreateLinks(string id, string incidentId, EventFilters filter, PaginationMetadata pagination, string routeName)
        {
            var _operationLinks = new OperationLinks()
            {
                Single = new SingleOperationLinks()
                {
                    Get = string.IsNullOrWhiteSpace(incidentId)
                        ? _urlHelper.Link(GetSingleChildRouteName, new { id })
                        : _urlHelper.Link(GetSingleChildRouteName, new { id, incidentId }),
                    Post = string.IsNullOrWhiteSpace(incidentId)
                        ? _urlHelper.Link(PostSingleChildRouteName, new { })
                        : _urlHelper.Link(PostSingleChildRouteName, new { incidentId })

                },
                Multiple = new MultipleOperationLinks()
                {
                    Get = string.IsNullOrWhiteSpace(incidentId)
                        ? _urlHelper.Link(GetChildrenRouteName, new { })
                        : _urlHelper.Link(GetChildrenRouteName, new { incidentId })
                }
            };
            var _relationLinks = new RelationLinks()
            {
                Parent = string.IsNullOrWhiteSpace(incidentId)
                    ? new RelatedParentLinks()
                    : new RelatedParentLinks()
                    {
                        Incident = _urlHelper.Link(IncidentsController.GetSingleRouteName, new { id = incidentId })
                    }
            };
            return new LinksHeader(filter, pagination, _urlHelper, routeName, _operationLinks, _relationLinks);
        }

        public const string GetEventsRouteName = "GetEvents";
        [HttpGet("events", Name = GetEventsRouteName)]
        public async Task<IActionResult> GetEvents(
            [FromQuery]PaginationMetadata pagination,
            [FromQuery]EventFilters filter)
        {
            var result = await _mediator.Send(new GetChildEventsRequest(incidentId, pagination, filter, _authContext));

            Response.Headers.AddLinksHeader(CreateLinks(null, null, filter, pagination, GetChildrenRouteName));

            return Ok(result);
        }

        public const string GetChildrenRouteName = "GetChildEvents";
        [HttpGet("incidents/{incidentId}/events", Name = GetChildrenRouteName)]
        public async Task<IActionResult> GetChildEvents([FromRoute]long incidentId,
            [FromQuery]PaginationMetadata pagination,
            [FromQuery]EventFilters filter)
        {
            var result = await _mediator.Send(new GetChildEventsRequest(incidentId, pagination, filter, _authContext));
            
            Response.Headers.AddLinksHeader(CreateLinks(null, incidentId.ToString(), filter, pagination, GetChildrenRouteName));

            return Ok(result);
        }



        public const string GetSingleChildRouteName = "GetChildEvent";
        [HttpGet("incidents/{incidentId}/events/{id}", Name = GetSingleChildRouteName)]
        public async Task<IActionResult> GetChild([FromRoute]long incidentId, [FromRoute]long id)
        {
            var result = await _mediator.Send(new GetChildEventRequest(incidentId, id, _authContext));

            Response.Headers.AddLinksHeader(CreateLinks(id.ToString(), incidentId.ToString(), null, null, GetSingleChildRouteName));

            return OkIfFound(result);
        }

        public const string PostSingleChildRouteName = "PostChildEvent";
        [HttpPost("incidents/{incidentId}/events", Name = PostSingleChildRouteName)]
        public async Task<IActionResult> PostChild([FromRoute]long incidentId, [FromBody]NewEvent newEvent)
        {
            var result = await _mediator.Send(new PostChildEventRequest(incidentId, newEvent, _authContext));
            if (result == null)
            {
                throw new ServerFailureException("Failed to add Event to parent incident."); 
            }
            await SendEventToSubscribers(result);

            Response.Headers.AddLinksHeader(CreateLinks(result.Id.ToString(), incidentId.ToString(), null, null, PostSingleChildRouteName));
            return Created(_urlHelper.Link(GetSingleChildRouteName, new { id = result.Id }), result);
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
