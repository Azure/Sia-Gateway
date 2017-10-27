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
    [Route("/actions/{actionId}/conditionSets/{conditionSetId}/condtions/")]
    public class ConditionController : BaseController
    {
        public ConditionController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, IUrlHelper urlHelper)
            : base(mediator, authConfig, urlHelper)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id, long conditionSetId)
            => Ok(await _mediator.Send(new GetConditionRequest(id, conditionSetId, _authContext)));

        [HttpPost()]
        public async Task<IActionResult> Post(CreateCondition content, long conditionSetId)
            => Ok(await _mediator.Send(new PostConditionRequest(conditionSetId, content,  _authContext)));
    }
}