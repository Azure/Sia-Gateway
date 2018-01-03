using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain;
using Sia.Gateway.Initialization;
using Sia.Shared.Protocol;
using Sia.Gateway.Requests;
using Sia.Gateway.Requests.Events;
using Sia.Gateway.Tests.TestDoubles;
using System.Linq;
using System.Threading.Tasks;
using Sia.Shared.Data;
using Sia.Data.Incidents.Filters;
using Sia.Shared.Protocol.Pagination;

namespace Sia.Gateway.Tests.Requests
{

    [TestClass]
    public class GetEventsTests
    {
        [TestInitialize]
        public void ConfigureAutomapper()
            => AutoMapperStartup.InitializeAutomapper();

        [TestMethod]
        public async Task Handle_WhenEFReturnsSuccessful_ReturnCorrectEvents()
        {
            long[] expectedEventIds = { 1, 2 };
            long[] expectedEventTypeIds = { 1, 111 };

            var filters = new EventFilters();
            var serviceUnderTest = new GetEventsHandler(await MockFactory.IncidentContext("Get"));
            var request = new GetEventsRequest(1, new PaginationByFixedPageSizeRequest<Data.Incidents.Models.Event, Event>(), filters, new DummyAuthenticatedUserContext());


            var result = (await serviceUnderTest.Handle(request)).QueryResult;


            for (int i = 0; i < expectedEventTypeIds.Length; i++)
            {
                Assert.AreEqual(expectedEventIds[i], result[i].Id);
                Assert.AreEqual(expectedEventTypeIds[i], result[i].EventTypeId);
            }
        }
    }
}
