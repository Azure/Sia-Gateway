using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Shared.Authentication;
using Sia.Gateway.Requests;
using System.Threading.Tasks;
using Sia.Shared.Controllers;
using Sia.Shared.Protocol;

namespace Sia.Gateway.Controllers
{
    [Route("[controller]")]
    public class IncidentsController : BaseController
    {

        private RelationLinks _relationLinks;
        public IncidentsController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig, IUrlHelper urlHelper) 
            : base(mediator, authConfig, urlHelper)
        {
        }

        public void CreateLinks(string id) {
            _operationLinks.Single = new SingleOperationLinks()
            {
                Get = _urlHelper.Link(GetSingleRouteName, new {id}),
                Post = _urlHelper.Link(PostSingleRouteName, new { })
            };
            _operationLinks.Multiple = new MultipleOperationLinks()
            {
                Get = _urlHelper.Link(GetMultipleRouteName, new { })
            };
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
            CreateLinks(id.ToString());
            Response.Headers.AddLinksHeader(new LinksHeader(null, _urlHelper, "IncidentsController" + GetSingleRouteName, _operationLinks, _relationLinks));
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
            CreateLinks(null);
            Response.Headers.AddLinksHeader(new LinksHeader(pagination, _urlHelper, "IncidentsController" + GetSingleRouteName, _operationLinks, _relationLinks));
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
            CreateLinks(result.Id.ToString());
            Response.Headers.AddLinksHeader(new LinksHeader(null, _urlHelper, PostSingleRouteName, _operationLinks, _relationLinks));
            return Created(newUrl, result);
        }


    }
}
