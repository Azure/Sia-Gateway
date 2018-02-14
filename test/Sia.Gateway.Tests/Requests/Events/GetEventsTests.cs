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
using System.Threading;

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
            var serviceUnderTest = new GetChildEventsHandler(await MockFactory.IncidentContext(nameof(Handle_WhenEFReturnsSuccessful_ReturnCorrectEvents)));
            var request = new GetChildEventsRequest(1, new PaginationMetadata(), filters, new DummyAuthenticatedUserContext());


            var result = (await serviceUnderTest.Handle(request, new CancellationToken())).ToList();


            for (int i = 0; i < expectedEventTypeIds.Length; i++)
            {
                Assert.AreEqual(expectedEventIds[i], result[i].Id);
                Assert.AreEqual(expectedEventTypeIds[i], result[i].EventTypeId);
            }
        }
    }
}
