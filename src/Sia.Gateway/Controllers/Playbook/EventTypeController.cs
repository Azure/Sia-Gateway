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
using Sia.Domain.Playbook;

namespace Sia.Gateway.Controllers
{
    [Route("/eventTypes/")]
    public class EventTypeController : BaseController
    {
        public EventTypeController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, IUrlHelper urlHelper)
            : base(mediator, authConfig, urlHelper)
        {
        }

        [HttpGet("{id}", Name = nameof(Get) + nameof(EventType))]
        public async Task<IActionResult> Get(long id)
            => Ok(await _mediator.Send(new GetEventTypeRequest(id, _authContext)));

        [HttpPost()]
        public async Task<IActionResult> Post(CreateEventType content)
            => CreatedAtRoute(nameof(Get) + nameof(EventType), await _mediator.Send(new PostEventTypeRequest(content, _authContext)));

        [HttpPut("{eventTypeId}/actions/{actionId}")]
        public async Task<IActionResult> AssociateAction(long actionId, long eventTypeId)
        {
            await _mediator.Send(new AssociateActionWithEventTypeRequest(actionId, eventTypeId, _authContext));
            return Ok();
        }
    }
}
