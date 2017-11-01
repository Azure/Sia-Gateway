using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sia.Domain.ApiModels.Playbooks;
using Sia.Domain.Playbook;
using Sia.Gateway.Requests;
using Sia.Shared.Authentication;
using Sia.Shared.Controllers;
using System.Threading.Tasks;

namespace Sia.Gateway.Controllers
{
    [Route("/actions/")]
    public class ActionController : BaseController
    {
        public ActionController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, IUrlHelper urlHelper)
            : base(mediator, authConfig, urlHelper)
        {
        }

        [HttpGet("{id}", Name = nameof(Get) + nameof(Action))]
        public async Task<IActionResult> Get(long id)
            => Ok(await _mediator.Send(new GetActionRequest(id, _authContext)));

        [HttpPost()]
        public async Task<IActionResult> Post(CreateAction content)
            => CreatedAtRoute(nameof(Get) + nameof(Action), await _mediator.Send(new PostActionRequest(content, _authContext)));

        [HttpPut("{actionId}/eventTypes/{eventTypeId}")]
        public async Task<IActionResult> AssociateEventType(long actionId, long eventTypeId)
        {
            await _mediator.Send(new AssociateActionWithEventTypeRequest(actionId, eventTypeId, _authContext));
            return Ok();
        }
    }
}
