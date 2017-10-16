﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Authentication;
using Sia.Gateway.Requests;
using System.Threading.Tasks;

namespace Sia.Gateway.Controllers
{
    [Route("[controller]")]
    public class IncidentsController : BaseController
    {

        public IncidentsController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, ILoggerFactory loggerFactory, IUrlHelper urlHelper) 
            : base(mediator, authConfig, loggerFactory, urlHelper)
        {
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _mediator.Send(new GetIncidentRequest(id, _authContext));
            if (result == null)
            {
                return NotFound($"{nameof(Incident)} not found");
            }
            return Ok(result);
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetIncidentsRequest(_authContext));
            if (result == null)
            {
                return NotFound($"{nameof(Incident)}s not found");
            }
            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Post([FromBody]NewIncident incident)
        {
            var result = await _mediator.Send(new PostIncidentRequest(incident, _authContext));
            if (result == null)
            {
                return NotFound($"{nameof(Incident)} not found");
            }
            return Created($"/incidents/{result.Id}", result);
        }
    }
}
