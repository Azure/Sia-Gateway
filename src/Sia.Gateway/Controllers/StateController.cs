using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Requests;
using Sia.Core.Authentication;
using Sia.Core.Controllers;
using Sia.Core.Protocol;
using System.Threading.Tasks;
using System;
using Sia.Core.Validation;
using Sia.Gateway.Links;
using Sia.Gateway.Requests.State;

namespace Sia.Gateway.Controllers
{
    public class StateController : BaseController
    {
        public StateController(
            IMediator mediator,
            AzureActiveDirectoryAuthenticationInfo authConfig,
            IUrlHelper urlHelper
        ) : base(mediator, authConfig, urlHelper)
        {

        }

        [HttpGet("incidents/{incidentId}/state", Name = StateRoutes.GetSingle)]
        public async Task<IActionResult> Get(long incidentId)
            => OkIfFound(await _mediator
                .Send(new GetStateRequest(incidentId, AuthContext))
                .ConfigureAwait(continueOnCapturedContext: false)
            );
    }
}
