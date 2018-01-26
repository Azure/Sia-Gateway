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

        //public void CreateLinks(string id)
        //{
        //    var _operationLinks = new OperationLinks()
        //    {
        //        Single = new SingleOperationLinks()
        //        {
        //            Get = _urlHelper.Link(GetSingleRouteName, new { id }),
        //            Post = _urlHelper.Link(PostSingleRouteName, new { })
        //        },
        //        Multiple = new MultipleOperationLinks()
        //        {
        //            Get = _urlHelper.Link(GetMultipleRouteName, new { })
        //        }

        //    };

        //    if (id != null)
        //    {
        //        var _relationLinks = new RelationLinks()
        //        {
        //            Children = new RelatedChildLinks()
        //            {
        //                Events = _urlHelper.Link(EventsController.GetMultipleRouteName, new { incidentId = id })
        //            }
        //        };
        //    }
        //}

        public const string GetSingleRouteName = "GetIncident";
        [HttpGet("{id}", Name = GetSingleRouteName)]
        public async Task<IActionResult> Get(long id, [FromQuery]EventFilters filter)
        {
            var result = await _mediator.Send(new GetIncidentRequest(id, _authContext));
            if (result == null)
            {
                return NotFound($"{nameof(Incident)} not found");
            }
            //CreateLinks(id.ToString());
            //Response.Headers.AddLinksHeader(new FilteredLinksHeader(filter, null, _urlHelper, "IncidentsController" + GetSingleRouteName, _operationLinks, _relationLinks));
            Response.Headers.AddLinksHeader( new FilteredLinksHeader(
                filter, null, _urlHelper, "IncidentsController" + GetSingleRouteName, new OperationLinks()
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

                }, new RelationLinks()
                {
                    Children = new RelatedChildLinks()
                    {
                        Events = _urlHelper.Link(EventsController.GetMultipleRouteName, new { incidentId = id })
                    }
                })
            );
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
            //CreateLinks(null);
            //Response.Headers.AddLinksHeader(new LinksHeader(pagination, _urlHelper, "IncidentsController" + GetSingleRouteName, _operationLinks, _relationLinks));
            Response.Headers.AddLinksHeader( new LinksHeader(
                pagination, _urlHelper, "IncidentsController" + GetSingleRouteName, new OperationLinks()
            {
                Single = new SingleOperationLinks()
                {
                    Post = _urlHelper.Link(PostSingleRouteName, new { })
                },
                Multiple = new MultipleOperationLinks()
                {
                    Get = _urlHelper.Link(GetMultipleRouteName, new { })
                }

            }, null));
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
            //CreateLinks(result.Id.ToString());
            //Response.Headers.AddLinksHeader(new LinksHeader(null, _urlHelper, PostSingleRouteName, _operationLinks, _relationLinks));
            Response.Headers.AddLinksHeader(
                new LinksHeader(null, _urlHelper, PostSingleRouteName, new OperationLinks()
                {
                    Single = new SingleOperationLinks()
                    {
                        Get = _urlHelper.Link(GetSingleRouteName, new { result.Id }),
                        Post = _urlHelper.Link(PostSingleRouteName, new { })
                    },
                    Multiple = new MultipleOperationLinks()
                    {
                        Get = _urlHelper.Link(GetMultipleRouteName, new { })
                    }

                }, new RelationLinks()
                {
                    Children = new RelatedChildLinks()
                    {
                        Events = _urlHelper.Link(EventsController.GetMultipleRouteName, new { incidentId = result.Id })
                    }
                }
                ));
            return Created(newUrl, result);
        }


    }
}
