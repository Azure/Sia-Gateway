using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sia.Shared.Authentication;
using Sia.Gateway.Requests;
using Sia.Domain.ApiModels.Playbooks;
using Sia.Shared.Controllers;
using Sia.Shared.Exceptions;
using Sia.Domain.Playbook;
using System.Net;

namespace Sia.Gateway.Controllers
{
    [Route("/eventTypes/")]
    public class EventTypesController : BaseController
    {
        private const string notFoundMessage = "Event type not found";

        public EventTypesController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, IUrlHelper urlHelper)
            : base(mediator, authConfig, urlHelper)
        {
        }

        [HttpGet(Name = nameof(GetAll) + nameof(EventType))]
        public async Task<IActionResult> GetAll()
            => Ok(await _mediator.Send(new GetEventTypesRequest(_authContext)));

        [HttpGet("{id}", Name = nameof(Get) + nameof(EventType))]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var eventType = await _mediator.Send(new GetEventTypeRequest(id, _authContext));
                return Ok(eventType);
            }
            catch (NotFoundException ex)
            {
                return NotFound();
            }
        }
    }
}
