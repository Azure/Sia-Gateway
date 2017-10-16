using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.Protocol;
using Sia.Gateway.Hubs;
using Sia.Gateway.Requests;
using Sia.Gateway.Requests.Events;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.Extensions.Logging;

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
            ILoggerFactory loggerFactory,
            IUrlHelper urlHelper)
            : base(mediator, authConfig, loggerFactory, urlHelper)
        {
            _hubConnectionBuilder = hubConnectionBuilder;
        }

        [HttpGet(Name = nameof(GetEvents))]
        public async Task<IActionResult> GetEvents([FromRoute]long incidentId, [FromQuery]PaginationMetadata pagination)
        {
            var result = await _mediator.Send(new GetEventsRequest(incidentId, pagination, _authContext));
            Response.Headers.AddPagination(new LinksHeader(pagination, _urlHelper, nameof(GetEvents)));
            return Ok(result);
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
        //public async Task<IActionResult> Post([FromRoute]long incidentId, [FromBody]NewEvent newEvent)
        public async Task<IActionResult> Post([FromRoute]long incidentId)
        {
            var logger = _loggerFactory.CreateLogger("EventController");

            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var body = await reader.ReadToEndAsync();
                var msg = JsonConvert.DeserializeObject<ParticipantMessage>(body);
                logger.LogWarning($"received an event {body}");
            }
            
            var newEvent = new NewEvent();
            return Created($"api/incidents/{incidentId}/events/123", newEvent);

            /*
            var result = await _mediator.Send(new PostEventRequest(incidentId, newEvent, _authContext));
            if (result == null)
            {
                return NotFound(notFoundMessage);
            }
            await SendEventToSubscribers(result);
            return Created($"api/incidents/{result.IncidentId}/events/{result.Id}", result);
            */
        }

        private async Task SendEventToSubscribers(Domain.Event result)
        {
            var eventHubConnection = _hubConnectionBuilder
                    .WithUrl($"{Request.Scheme}://{Request.Host}/{EventsHub.HubPath}")
                    .Build();
            await eventHubConnection.StartAsync();
            await eventHubConnection.SendAsync("Send", result);
            eventHubConnection.DisposeAsync();
        }
    }

    public class ParticipantEvent
    {
        [Required]
        public string Alias { get; set; }
        [Required]
        public string Team { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Action { get; set; }
    }

    public class ParticipantMessage
    {
        [Required]
        [JsonProperty("siaEvent")]
        public NewEvent newEvent { get; set; }
        [Required]
        public ParticipantEvent participantEvent { get; set; }
    }
}
