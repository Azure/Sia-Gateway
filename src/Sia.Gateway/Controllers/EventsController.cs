using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Sia.Domain.ApiModels;
using Sia.Gateway.Filters;
using Sia.Gateway.Hubs;
using Sia.Gateway.Requests;
using Sia.Gateway.Requests.Events;
using Sia.Shared.Authentication;
using Sia.Shared.Controllers;
using Sia.Shared.Protocol;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sia.Gateway.Controllers
{
    public class EventsController : BaseController
    {
        private const string notFoundMessage = "Incident or event not found";
        private readonly HubConnectionBuilder _hubConnectionBuilder;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IMediator mediator,
            AzureActiveDirectoryAuthenticationInfo authConfig,
            HubConnectionBuilder hubConnectionBuilder,
            IUrlHelper urlHelper,
            ILoggerFactory loggerFactory)
            : base(mediator, authConfig, urlHelper)
        {
            _hubConnectionBuilder = hubConnectionBuilder;
            _logger = loggerFactory.CreateLogger<EventsController>();
        }

        public LinksHeader CreateLinks(string id, string incidentId, EventFilters filter, PaginationMetadata pagination, string routeName)
        {
            var _operationLinks = new OperationLinks()
            {
                Single = new SingleOperationLinks()
                {
                    Get = _urlHelper.Link(GetSingleRouteName, new { id }),
                    Post = _urlHelper.Link(PostSingleRouteName, new { })

                },
                Multiple = new MultipleOperationLinks()
                {
                    Get = _urlHelper.Link(GetMultipleRouteName, new { })
                }
            };
            var _relationLinks = new RelationLinks()
            {
                Parent = new RelatedParentLinks()
                {
                    Incident = _urlHelper.Link(IncidentsController.GetSingleRouteName, new { id = incidentId })
                }
            };
            return new LinksHeader(filter, pagination, _urlHelper, routeName, _operationLinks, _relationLinks);
        }

        public const string GetMultipleRouteName = "GetEvents";
        [HttpGet("incidents/{incidentId}/events", Name = GetMultipleRouteName)]
        public async Task<IActionResult> GetEvents([FromRoute]long incidentId,
            [FromQuery]PaginationMetadata pagination,
            [FromQuery]EventFilters filter)
        {
            var result = await _mediator.Send(new GetEventsRequest(incidentId, pagination, filter, _authContext));
            
            Response.Headers.AddLinksHeader(CreateLinks(null, incidentId.ToString(), filter, pagination, GetMultipleRouteName));

            return Ok(result);
        }

        public const string GetSingleRouteName = "GetEvent";
        [HttpGet("incidents/{incidentId}/events/{id}", Name = GetSingleRouteName)]
        public async Task<IActionResult> Get([FromRoute]long incidentId, [FromRoute]long id)
        {
            var result = await _mediator.Send(new GetEventRequest(incidentId, id, _authContext));

            Response.Headers.AddLinksHeader(CreateLinks(id.ToString(), incidentId.ToString(), null, null, GetSingleRouteName));
            if (result == null)
            {
                return NotFound(notFoundMessage);
            }

            return Ok(result);
        }

        public const string PostSingleRouteName = "PostEvent";
        [HttpPost("incidents/{incidentId}/events", Name = PostSingleRouteName)]
        public async Task<IActionResult> Post([FromRoute]long incidentId, [FromBody]NewEvent newEvent)
        {
            var result = await _mediator.Send(new PostEventRequest(incidentId, newEvent, _authContext));
            if (result == null)
            {
                return NotFound(notFoundMessage);
            }

            await SendEventToSubscribers(result);

            var newUrl = _urlHelper.Link(GetSingleRouteName, new { id = result.Id });

            Response.Headers.AddLinksHeader(CreateLinks(result.Id.ToString(), incidentId.ToString(),null, null, PostSingleRouteName));
            return Created(newUrl, result);
        }

        public const string GetMultipleUncorrelatedRouteName = "GetUncorrelatedEvent";
        [HttpGet("events", Name = GetMultipleUncorrelatedRouteName)]
        public async Task<IActionResult> GetUncorrelatedEvents([FromQuery]PaginationMetadata pagination,
            [FromQuery]EventFilters filter)
        {
            var result = await _mediator.Send(new GetUncorrelatedEventsRequest(pagination, filter, _authContext));
            return Ok(result);
        }

        private async Task SendEventToSubscribers(Domain.Event result)
        {
            var url = $"http://localhost:{Request.Host.Port}{EventsHub.HubPath}";
            try
            {
                string token = GetTokenFromHeaders();
                var eventHubConnection = _hubConnectionBuilder
                    .WithAccessToken(() => token)
                    .WithUrl(url)
                    .Build();
                await eventHubConnection.StartAsync();
                await eventHubConnection.SendAsync("Send", result);
                await eventHubConnection.DisposeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Encountered exception when attempting to send posted event to SignalR subscribers, url: {url}");
            }
        }

        private string GetTokenFromHeaders()
            => HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        // Found by reading source code at
        // https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNetCore.Authentication.JwtBearer/JwtBearerHandler.cs
        private const string AccessTokenName = "access_token";
    }
}
