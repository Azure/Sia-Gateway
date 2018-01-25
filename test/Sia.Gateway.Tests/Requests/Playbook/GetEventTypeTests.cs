using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain;
using Sia.Gateway.Initialization;
using Sia.Gateway.Requests;
using Sia.Gateway.Tests.TestDoubles;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Requests.Playbook
{
    [TestClass]
    class GetEventTypeTests
    {
        //[TestMethod]
        //public async Task Handle_WhenEventTypeClientReturnsSuccessful_ReturnCorrectEventType()
        //{
        //    long expectedEventId = 1;
        //    long expectedEventTypeId = 1;
        //    long expectedIncidentId = 1;
        //    var expectedEvent = new Event
        //    {
        //        Id = expectedEventId,
        //        EventTypeId = expectedEventTypeId,
        //        IncidentId = expectedIncidentId
        //    };
        //    const string PlaybookEndpointName = "Playbook";

        //    var serviceUnderTest = new GetEventTypeHandler(await (nameof(Handle_WhenEventTypeClientReturnsSuccessful_ReturnCorrectEventType)));
        //    var request = new GetEventTypeRequest(expectedIncidentId, expectedEventId, new DummyAuthenticatedUserContext());


        //    var result = await serviceUnderTest.Handle(request, new System.Threading.CancellationToken());


        //    Assert.AreEqual(expectedEventId, result.Id);
        //    Assert.AreEqual(expectedEventTypeId, result.EventTypeId);
        //    Assert.AreEqual(expectedIncidentId, result.IncidentId.Value);
        //}
    }
}
