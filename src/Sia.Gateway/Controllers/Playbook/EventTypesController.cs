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
    [Route("/eventTypes/")]
    public class EventTypesController : BaseController
    {
        public EventTypesController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, IUrlHelper urlHelper)
            : base(mediator, authConfig, urlHelper)
        {
        }

        [HttpGet(Name = nameof(GetAll) + nameof(EventType))]
        public async Task<IActionResult> GetAll() 
            => OkIfFound(await _mediator
                .Send(new GetEventTypesRequest(AuthContext))
                .ConfigureAwait(continueOnCapturedContext: false));


        [HttpGet("{id}", Name = nameof(Get) + nameof(EventType))]
        public async Task<IActionResult> Get(long id)
            => OkIfFound(await _mediator
                .Send(new GetEventTypeRequest(id, AuthContext))
                .ConfigureAwait(continueOnCapturedContext: false));
    }
}
