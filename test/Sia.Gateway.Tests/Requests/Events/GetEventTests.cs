using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain;
using Sia.Gateway.Initialization;
using Sia.Gateway.Requests;
using Sia.Gateway.Tests.TestDoubles;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Requests
{
    [TestClass]
    public class GetEventTests
    {
        [TestInitialize]
        public void ConfigureAutomapper()
            => AutoMapperStartup.InitializeAutomapper();

        [TestMethod]
        public async Task Handle_WhenEventClientReturnsSuccessful_ReturnCorrectEvent()
        {
            long expectedEventId = 1;
            long expectedEventTypeId = 1;
            long expectedIncidentId = 1;
            var expectedEvent = new Event
            {
                Id = expectedEventId,
                EventTypeId = expectedEventTypeId,
                IncidentId = expectedIncidentId
            };
            var serviceUnderTest = new GetEventHandler(await MockFactory
                .IncidentContext(nameof(Handle_WhenEventClientReturnsSuccessful_ReturnCorrectEvent))
                .ConfigureAwait(continueOnCapturedContext: false));
            var request = new GetEventRequest(expectedIncidentId, expectedEventId, new DummyAuthenticatedUserContext());


            var result = await serviceUnderTest
                .Handle(request, new System.Threading.CancellationToken())
                .ConfigureAwait(continueOnCapturedContext: false);


            Assert.AreEqual(expectedEventId, result.Id);
            Assert.AreEqual(expectedEventTypeId, result.EventTypeId);
            Assert.AreEqual(expectedIncidentId, result.IncidentId.Value);
        }
    }
}