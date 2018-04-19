using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sia.Core.Authentication;
using Sia.Gateway.Requests;
using Sia.Domain.ApiModels.Playbooks;
using Sia.Core.Controllers;
using Sia.Domain.Playbook;

namespace Sia.Gateway.Controllers
{
    [Route("/globalActions/")]
    public class GlobalActionsController : BaseController
    {
        public GlobalActionsController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, IUrlHelper urlHelper)
            : base(mediator, authConfig, urlHelper)
        {
        }

        [HttpGet(Name = nameof(GetAll) + "Global" + nameof(Domain.Playbook.Action))]
        public async Task<IActionResult> GetAll()
            => OkIfFound(await _mediator
                .Send(new GetGlobalActionsRequest(authContext))
                .ConfigureAwait(continueOnCapturedContext: false));
    }
}
