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

namespace Sia.Gateway.Controllers
{
    [Route("/actions/{actionId}/conditionSets/")]
    public class ConditionSetController : BaseController
    {
        public ConditionSetController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, IUrlHelper urlHelper)
            : base(mediator, authConfig, urlHelper)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id, long actionId)
            => Ok(await _mediator.Send(new GetConditionSetRequest(id, actionId, _authContext)));

        [HttpPost()]
        public async Task<IActionResult> Post(CreateConditionSet content, long actionId)
            => Ok(await _mediator.Send(new PostConditionSetRequest(actionId, content,  _authContext)));
    }
}