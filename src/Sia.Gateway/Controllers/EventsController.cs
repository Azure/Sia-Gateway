using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Sia.Domain.ApiModels;
using Sia.Shared.Authentication;
using Sia.Shared.Protocol;
using Sia.Gateway.Hubs;
using Sia.Gateway.Requests;
using Sia.Gateway.Requests.Events;
using System.Threading.Tasks;
using Sia.Shared.Controllers;
using Sia.Shared.Data;
using Sia.Data.Incidents.Filters;

namespace Sia.Gateway.Controllers
{
    [Route("incidents/{incidentId}/events", Name = "Events")]
    public class EventsController : BaseController
    {
        private const string notFoundMessage = "Incident or event not found";
        private readonly HubConnectionBuilder _hubConnectionBuilder;

        public EventsController(IMediator mediator,
            AzureActiveDirectoryAuthenticationInfo authConfig,
            HubConnectionBuilder hubConnectionBuilder,
            IUrlHelper urlHelper)
            : base(mediator, authConfig, urlHelper)
        {
            _hubConnectionBuilder = hubConnectionBuilder;
        }

        //public void CreateLinks(string id, string incidentId)
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
        //    var _relationLinks = new RelationLinks()
        //    {
        //        Parent = new RelatedParentLinks()
        //        {
        //            Incident = _urlHelper.Link(IncidentsController.GetSingleRouteName, new { id = incidentId })
        //        }
        //    };
        //}

        public const string GetMultipleRouteName = "GetEvents";
        [HttpGet(Name = GetMultipleRouteName)]
        public async Task<IActionResult> GetEvents([FromRoute]long incidentId,
            [FromQuery]PaginationMetadata pagination,
            [FromQuery]EventFilters filter)
        {
            var result = await _mediator.Send(new GetEventsRequest(incidentId, pagination, filter, _authContext));

            //CreateLinks(null, incidentId.ToString());
            //Response.Headers.AddLinksHeader(new FilteredLinksHeader(filter, pagination, _urlHelper, GetMultipleRouteName, _operationLinks, _relationLinks));
            Response.Headers.AddLinksHeader(new FilteredLinksHeader(filter, pagination, _urlHelper, GetMultipleRouteName, new OperationLinks()
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

        public const string GetSingleRouteName = "GetEvent";
        [HttpGet("{id}", Name = GetSingleRouteName)]
        public async Task<IActionResult> Get([FromRoute]long incidentId, [FromRoute]long id)
        {
            var result = await _mediator.Send(new GetEventRequest(incidentId, id, _authContext));

            //CreateLinks(id.ToString(), incidentId.ToString());
            //Response.Headers.AddLinksHeader(new LinksHeader(null, _urlHelper, GetSingleRouteName, _operationLinks, _relationLinks));
            Response.Headers.AddLinksHeader(new LinksHeader(null, _urlHelper, GetSingleRouteName,
            new OperationLinks()
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
            },
                new RelationLinks()
                {
                    Parent = new RelatedParentLinks()
                    {
                        Incident = _urlHelper.Link(IncidentsController.GetSingleRouteName, new { id = incidentId })
                    }
                }
            ));
            if (result == null)
            {
                return NotFound(notFoundMessage);
            }

            return Ok(result);
        }

        public const string PostSingleRouteName = "PostEvent";
        [HttpPost(Name = PostSingleRouteName)]
        public async Task<IActionResult> Post([FromRoute]long incidentId, [FromBody]NewEvent newEvent)
        {
            var result = await _mediator.Send(new PostEventRequest(incidentId, newEvent, _authContext));
            if (result == null)
            {
                return NotFound(notFoundMessage);
            }
            await SendEventToSubscribers(result);

            var newUrl = _urlHelper.Link(GetSingleRouteName, new { id = result.Id });

            //CreateLinks(result.Id.ToString(), incidentId.ToString());
            //Response.Headers.AddLinksHeader(new LinksHeader(null, _urlHelper, PostSingleRouteName, _operationLinks, _relationLinks));
            Response.Headers.AddLinksHeader(new LinksHeader(null, _urlHelper, PostSingleRouteName,
                new OperationLinks()
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
                },
            new RelationLinks()
            {
                Parent = new RelatedParentLinks()
                {
                    Incident = _urlHelper.Link(IncidentsController.GetSingleRouteName, new { id = incidentId })
                }
            }
                ));
            return Created(newUrl, result);
        }

        private async Task SendEventToSubscribers(Domain.Event result)
        {
            var eventHubConnection = _hubConnectionBuilder
                    .WithUrl($"{Request.Scheme}://{Request.Host}/{EventsHub.HubPath}")
                    .Build();
            await eventHubConnection.StartAsync();
            await eventHubConnection.SendAsync("Send", result);
            await eventHubConnection.DisposeAsync();
        }


    }
}
