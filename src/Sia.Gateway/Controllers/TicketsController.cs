using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sia.Domain;
using Sia.Gateway.Authentication;
using Sia.Gateway.Requests;
using System.Threading.Tasks;


namespace Sia.Gateway.Controllers
{
    [Route("[controller]")]
    public class TicketsController : BaseController
    {
        public TicketsController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig)
            : base(mediator, authConfig)
        {
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _mediator.Send(new GetIncidentsByTicketRequest(id, new AuthenticatedUserContext(User, HttpContext.Session, _authConfig)));
            if (result == null)
            {
                return NotFound($"{nameof(Incident)} not found");
            }
            return Ok(result);
        }
    }
}
