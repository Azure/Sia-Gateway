using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;


using Sia.Gateway.Requests.Events;
using Sia.Gateway.Tests.TestDoubles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Sia.Shared.Protocol;
using Sia.Gateway.Controllers;
using System.Threading.Tasks;


namespace Sia.Gateway.Tests.Requests.Events
{
    [TestClass]
    class GetEventsHeaderLinksTest
    {
        [TestMethod]
        public void EventsHeaderLinksGenerationTest(IUrlHelper urlHelper)
        {
            var eventsController = new EventsController(null, null, null, null);
            eventsController.CreateLinks(null, "1");
            eventsController.Response.Headers.AddLinksHeader(new FilteredLinksHeader(null, new PaginationMetadata(), urlHelper, "EventsController.GetEvents", eventsController._operationLinks, eventsController._relationLinks));
            
        }
    }
}
