using Microsoft.AspNetCore.Mvc;
using Sia.Core.Protocol;
using Sia.Core.Validation;
using Sia.Gateway.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Links
{
    public static class EventRoutes
    {
        public const string GetMultiple = "GetUncorrelatedEvent";
    }

    public static class EventRoutesByIncident
    {
        public const string GetMultiple = "GetEvents";
        public const string GetSingle = "GetEvent";
        public const string PostSingle = "PostEvent";
    }

    public class EventLinksProvider : LinksProvider
    {
        public EventLinksProvider(IUrlHelper urlHelper)
            : base(urlHelper) { }

        protected override OperationLinks GetOperationLinks(object routeValues)
            => new OperationLinks()
            {
                Single = new SingleOperationLinks()
                {
                    Get = UrlHelper.Link(EventRoutesByIncident.GetSingle, routeValues),
                    Post = UrlHelper.Link(EventRoutesByIncident.PostSingle, routeValues)

                },
                Multiple = new MultipleOperationLinks()
                {
                    Get = UrlHelper.Link(EventRoutesByIncident.GetMultiple, routeValues)
                }
            };

        public ILinksHeader CreatePaginatedLinks(string routeName, PaginationMetadata pagination, EventFilters filter, long incidentId = default(long))
        {
            ThrowIf.NullOrWhiteSpace(routeName, nameof(routeName));
            var routeValues = (incidentId == default(long))
                ? (object)new { }
                : new { incidentId = incidentId.ToPathTokenString() };
            var route = UrlHelper.Link(routeName, routeValues);

            var operationLinks = GetOperationLinks(routeValues);
            RelationLinks relationLinks = null;

            return new PaginatedLinksHeader(route, operationLinks, relationLinks, pagination, filter);
        }

        public ILinksHeader CreateLinks(long incidentId, long eventId)
        {
            var routeValues = new
            {
                incidentId = incidentId.ToPathTokenString(),
                eventId = eventId.ToPathTokenString()
            };

            var operationLinks = GetOperationLinks(routeValues);
            var relationLinks = new RelationLinks()
            {
                Parent = new EventParentLinks()
                {
                    Incident = UrlHelper.Link(IncidentRoutes.GetSingle, new { incidentId })
                }
            };

            return new CrudLinksHeader(operationLinks, relationLinks);
        }
    }

    public class EventParentLinks
    {
        public string Incident { get; set; }
    }
}
