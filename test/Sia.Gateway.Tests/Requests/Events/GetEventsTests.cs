using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Core.Protocol;
using Sia.Gateway.Filters;
using Sia.Gateway.Initialization;
using Sia.Gateway.Requests.Events;
using Sia.Gateway.Tests.TestDoubles;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            var serviceUnderTest = new GetEventsHandler(await MockFactory
                .IncidentContext(nameof(Handle_WhenEFReturnsSuccessful_ReturnCorrectEvents))
                .ConfigureAwait(continueOnCapturedContext: false));
            var request = new GetEventsRequest(1, new PaginationMetadata(), filters, new DummyAuthenticatedUserContext());


            var result = (await serviceUnderTest
                .Handle(request, new CancellationToken())
                .ConfigureAwait(continueOnCapturedContext: false)).ToList();


            for (int i = 0; i < expectedEventTypeIds.Length; i++)
            {
                Assert.AreEqual(expectedEventIds[i], result[i].Id);
                Assert.AreEqual(expectedEventTypeIds[i], result[i].EventTypeId);
            }
        }
    }
}
