using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Shared.Authentication;
using Sia.Gateway.Requests;
using System.Threading.Tasks;
using Sia.Data.Incidents.Filters;
using Sia.Shared.Controllers;
using Sia.Shared.Protocol;

namespace Sia.Gateway.Controllers
{
    [Route("[controller]")]
    public class IncidentsController : BaseController
    {
        public IncidentsController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, IUrlHelper urlHelper) 
            : base(mediator, authConfig, urlHelper)
        {
        }

        public LinksHeader CreateLinks(string id, PaginationMetadata pagination, string routeName)
        {
            var _operationLinks = new OperationLinks()
            {
                Single = new SingleOperationLinks()
                {
                    Get = _urlHelper.Link(GetSingleRouteName, new { id }),
                    Post = _urlHelper.Link(PostSingleRouteName, new { })
                },
                Multiple = new MultipleOperationLinks()
                {
                    Get = _urlHelper.Link(GetMultipleRouteName, new { })
                }

            };
            RelationLinks _relationLinks = null;
            if (id != null)
            {
                 _relationLinks = new RelationLinks()
                {
                    Children = new RelatedChildLinks()
                    {
                        Events = _urlHelper.Link(EventsController.GetMultipleRouteName, new { incidentId = id })
                    }
                };
            }

            return new LinksHeader(null, pagination, _urlHelper, routeName, _operationLinks, _relationLinks);
        }

        public const string GetSingleRouteName = "GetIncident";
        [HttpGet("{id}", Name = GetSingleRouteName)]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _mediator.Send(new GetIncidentRequest(id, _authContext));
            if (result == null)
            {
                return NotFound($"{nameof(Incident)} not found");
            }
            Response.Headers.AddLinksHeader(CreateLinks(id.ToString(), null, GetSingleRouteName));
       
            return Ok(result);
        }

        public const string GetMultipleRouteName = "GetIncidents";
        [HttpGet(Name = GetMultipleRouteName)]
        public async Task<IActionResult> Get([FromQuery] PaginationMetadata pagination)
        {
            var result = await _mediator.Send(new GetIncidentsRequest(pagination, _authContext));
            if (result == null)
            {
                return NotFound($"{nameof(Incident)}s not found");
            }
            Response.Headers.AddLinksHeader(CreateLinks(null, pagination, GetSingleRouteName));
            return Ok(result);
        }

        public const string PostSingleRouteName = "PostIncident";
        [HttpPost(Name = PostSingleRouteName)]
        public async Task<IActionResult> Post([FromBody]NewIncident incident)
        {
            var result = await _mediator.Send(new PostIncidentRequest(incident, _authContext));
            if (result == null)
            {
                return NotFound($"{nameof(Incident)} not found");
            }
            var newUrl = _urlHelper.Link(EventsController.GetMultipleRouteName, new { incidentId = result.Id });
            Response.Headers.AddLinksHeader(CreateLinks(result.Id.ToString(), null, PostSingleRouteName));
            return Created(newUrl, result);
        }


    }
}
