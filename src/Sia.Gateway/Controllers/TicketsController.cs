using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sia.Domain;
using Sia.Gateway.Authentication;
using Sia.Gateway.Requests;
using System.Threading.Tasks;


namespace Sia.Gateway.Controllers
{
    [Route("[controller]")]
    public class TicketsController : BaseController
    {
        public TicketsController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, ILoggerFactory loggerFactory, IUrlHelper urlHelper) 
            : base(mediator, authConfig, loggerFactory, urlHelper)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _mediator.Send(new GetIncidentsByTicketRequest(id, _authContext));
            if (result == null)
            {
                return NotFound($"{nameof(Incident)} not found");
            }
            return Ok(result);
        }
    }
}
