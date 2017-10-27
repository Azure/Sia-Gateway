using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Shared.Authentication;
using Sia.Gateway.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sia.Shared.Controllers;

namespace Sia.Gateway.Controllers
{
    [Route("[controller]")]
    public class TicketsController : BaseController
    {
        public TicketsController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, IUrlHelper urlHelper) 
            : base(mediator, authConfig, urlHelper)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _mediator.Send(new GetIncidentsByTicketCreateIfNeededRequest(id, _authContext));
            return Ok(result);
        }
    }
}
