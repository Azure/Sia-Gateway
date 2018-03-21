using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Gateway.Filters;
using Sia.Gateway.Initialization;
using Sia.Gateway.Requests.Events;
using Sia.Gateway.Tests.TestDoubles;
using Sia.Shared.Protocol;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Requests.Events
{
    [TestClass]
    public class GetUncorrelatedEventsTest
    {
        [TestInitialize]
        public void ConfigureAutomapper()
            => AutoMapperStartup.InitializeAutomapper();

        [TestMethod]
        public async Task Handle_WhenEFReturnsSuccessful_ReturnCorrectEvents()
        {
            long[] expectedEventIds = { 1, 2 };
            long[] expectedEventTypeIds = { 1, 111 };
            var filters = new EventFilters()
            {
                IncidentId = 1
            };
            
            var serviceUnderTest = new GetUncorrelatedEventsHandler(await MockFactory.IncidentContext(nameof(Handle_WhenEFReturnsSuccessful_ReturnCorrectEvents)));
            var request = new GetUncorrelatedEventsRequest(new PaginationMetadata(), filters, new DummyAuthenticatedUserContext());

            var result = (await serviceUnderTest.Handle(request, new CancellationToken())).ToList();


            for (int i = 0; i < expectedEventTypeIds.Length; i++)
            {
                Assert.AreEqual(expectedEventIds[i], result[i].Id);
                Assert.AreEqual(expectedEventTypeIds[i], result[i].EventTypeId);
            }
        }
    }
}

