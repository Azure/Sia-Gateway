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
using Sia.State.Services.Demo;

namespace Sia.Gateway.Controllers
{
    public class DemoController : BaseController
    {
        public DemoController(
            IMediator mediator,
            AzureActiveDirectoryAuthenticationInfo authConfig,
            IUrlHelper urlHelper
        ) : base(mediator, authConfig, urlHelper)
        {

        }

        [HttpGet("demo/{eventIndex}")]
        public Task<IActionResult> Get(int eventIndex)
            => Task.FromResult((IActionResult)Ok(new DemoEventsService().Events[eventIndex]));

    }
}
