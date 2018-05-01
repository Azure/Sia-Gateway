using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Sia.Core.Protocol;
using Sia.Core.Validation;

namespace Sia.Gateway.Links
{
    public static class IncidentRoutes
    {
        public const string GetSingle = "GetIncident";
        public const string GetMultiple = "GetIncidents";
        public const string PostSingle = "PostIncident";
    }
    public class IncidentLinksProvider : LinksProvider
    {
        public IncidentLinksProvider(IUrlHelper urlHelper)
            : base(urlHelper) { }

        protected override OperationLinks GetOperationLinks(object routeValues)
            => new OperationLinks()
            {
                Single = new SingleOperationLinks()
                {
                    Get = UrlHelper.Link(IncidentRoutes.GetSingle, routeValues),
                    Post = UrlHelper.Link(IncidentRoutes.PostSingle, routeValues)
                },
                Multiple = new MultipleOperationLinks()
                {
                    Get = UrlHelper.Link(IncidentRoutes.GetMultiple, routeValues)
                }
            };
        public ILinksHeader CreatePaginatedLinks(string routeName, PaginationMetadata pagination)
        {
            ThrowIf.NullOrWhiteSpace(routeName, nameof(routeName));
            var routeValues = new { };
            var route = UrlHelper.Link(routeName, routeValues);

            var operationLinks = GetOperationLinks(routeValues);
            RelationLinks relationLinks = null;

            return new PaginatedLinksHeader(route, operationLinks, relationLinks, pagination);
        }

        public ILinksHeader CreateLinks(long incidentId)
        {
            var routeValues = new { incidentId = incidentId.ToPathTokenString() };
            var operationLinks = GetOperationLinks(routeValues);
            var relationLinks = new RelationLinks()
            {
                Children = new IncidentChildLinks()
                {
                    Events = UrlHelper.Link(EventRoutesByIncident.GetMultiple, routeValues)
                }
            };

            return new CrudLinksHeader(operationLinks, relationLinks);
        }
    }
    public class IncidentChildLinks
    {
        public string Events { get; set; }
    }
}
