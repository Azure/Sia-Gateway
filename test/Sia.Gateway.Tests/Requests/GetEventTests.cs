using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain;
using Sia.Gateway.Requests;
using Sia.Gateway.ServiceRepositories;
using Sia.Gateway.Tests.TestDoubles;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Requests
{
    [TestClass]
    public class GetEventTests
    {
        [TestMethod]
        public async Task Handle_WhenEventClientReturnsSuccessful_ReturnCorrectEvent()
        {
            long expectedEventId = 200;
            long expectedEventTypeId = 50;
            long expectedIncidentId = 2;
            var expectedEvent = new Event
            {
                Id = expectedEventId,
                EventTypeId = expectedEventTypeId,
                IncidentId = expectedIncidentId
            };
            IEventRepository mockRepository = new StubEventRepository(expectedEvent);
            var serviceUnderTest = new GetEventHandler(mockRepository);
            var request = new GetEventRequest(expectedIncidentId, expectedEventId, new DummyAuthenticatedUserContext());


            var result = await serviceUnderTest.Handle(request);


            Assert.AreEqual(expectedEventId, result.Id);
            Assert.AreEqual(expectedEventTypeId, result.EventTypeId);
            Assert.AreEqual(expectedIncidentId, result.IncidentId.Value);
        }
    }
}