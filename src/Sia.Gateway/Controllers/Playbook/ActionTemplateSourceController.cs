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
    [Route("/actionTemplates/{actionTemplateId}/actionTemplateSources/")]
    public class ActionTemplateSourceController : BaseController
    {
        public ActionTemplateSourceController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, IUrlHelper urlHelper)
            : base(mediator, authConfig, urlHelper)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id, long actionTemplateId)
            => Ok(await _mediator.Send(new GetActionTemplateSourceRequest(id, actionTemplateId, _authContext)));

        [HttpPost()]
        public async Task<IActionResult> Post(CreateActionTemplateSource content, long actionTemplateId)
            => Ok(await _mediator.Send(new PostActionTemplateSourceRequest(actionTemplateId, content,  _authContext)));
    }
}