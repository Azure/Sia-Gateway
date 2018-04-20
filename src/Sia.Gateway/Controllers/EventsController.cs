using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Sia.Domain.ApiModels;
using Sia.State.Filters;
using Sia.Gateway.Hubs;
using Sia.Gateway.Requests;
using Sia.Gateway.Requests.Events;
using Sia.Core.Authentication;
using Sia.Core.Controllers;
using Sia.Core.Protocol;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Sia.Gateway.Links;
using Sia.Core.Validation;
using Sia.Domain;

namespace Sia.Gateway.Controllers
{
    public class EventsController : BaseController
    {
        public EventLinksProvider Links { get; }

        public EventsController(IMediator mediator,
            AzureActiveDirectoryAuthenticationInfo authConfig,
            IUrlHelper urlHelper,
            EventLinksProvider links)
            : base(mediator, authConfig, urlHelper)
        {
            Links = links;
        }
        
        [HttpGet("incidents/{incidentId}/events", Name = EventRoutesByIncident.GetMultiple)]
        public async Task<IActionResult> GetEvents([FromRoute]long incidentId,
            [FromQuery]PaginationMetadata pagination,
            [FromQuery]EventFilters filter)
        {
            var result = await _mediator
                .Send(new GetEventsRequest(incidentId, pagination, filter, AuthContext))
                .ConfigureAwait(continueOnCapturedContext: false);
            
            var links = Links.CreatePaginatedLinks(EventRoutesByIncident.GetMultiple, pagination, filter, incidentId);

            return OkIfFound(result, links);
        }

        [HttpGet("incidents/{incidentId}/events/{eventId}", Name = EventRoutesByIncident.GetSingle)]
        public async Task<IActionResult> Get([FromRoute]long incidentId, [FromRoute]long eventId)
        {
            var result = await _mediator
                .Send(new GetEventRequest(incidentId, eventId, AuthContext))
                .ConfigureAwait(continueOnCapturedContext: false);

            var links = Links.CreateLinks(incidentId, eventId);

            return OkIfFound(result, links);
        }

        [HttpPost("incidents/{incidentId}/events", Name = EventRoutesByIncident.PostSingle)]
        public async Task<IActionResult> Post([FromRoute]long incidentId, [FromBody]NewEvent newEvent)
        {
            var result = await _mediator
                .Send(new PostEventRequest(incidentId, newEvent, AuthContext, new Lazy<string>(() => GetTokenFromHeaders()), Request.Host.Port))
                .ConfigureAwait(continueOnCapturedContext: false);

            ILinksHeader getLinks(Event res) =>
                Links.CreateLinks(incidentId, res.Id);

            Uri getRetrievalRoute(Event res) =>
                new Uri(_urlHelper.Link(EventRoutesByIncident.GetSingle, new { incidentId, eventId = res.Id }));

            return CreatedIfExists(result, getRetrievalRoute, getLinks);
        }

        [HttpGet("events", Name = EventRoutes.GetMultiple)]
        public async Task<IActionResult> GetUncorrelatedEvents(
            [FromQuery]PaginationMetadata pagination,
            [FromQuery]EventFilters filter
        )
        {
            var result = await _mediator
                .Send(new GetUncorrelatedEventsRequest(pagination, filter, AuthContext))
                .ConfigureAwait(continueOnCapturedContext: false);

            var links = Links.CreatePaginatedLinks(EventRoutes.GetMultiple, pagination, filter);

            return Ok(result);
        }

        private string GetTokenFromHeaders()
            => HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
    }
}
