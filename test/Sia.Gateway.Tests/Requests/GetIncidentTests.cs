using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain;
using Sia.Gateway.Requests;
using Sia.Gateway.ServiceRepositories;
using Sia.Gateway.Tests.TestDoubles;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Requests
{
    [TestClass]
    public class GetIncidentTests
    {
        [TestMethod]
        public async Task Handle_WhenIncidentClientReturnsSuccessful_ReturnCorrectIncident()
        {
            long expectedIncidentId = 200;
            string expectedIncidentTitle = "The thing we were looking for";
            var expectedIncident = new Incident
            {
                Id = expectedIncidentId,
                Title = expectedIncidentTitle
            };
            IIncidentRepository mockClient = new StubIncidentRepository(expectedIncident, null);
            var serviceUnderTest = new GetIncidentHandler(mockClient);
            var request = new GetIncidentRequest(expectedIncidentId, new DummyAuthenticatedUserContext());


            var result = await serviceUnderTest.Handle(request);


            Assert.AreEqual(expectedIncidentId, result.Id);
            Assert.AreEqual(expectedIncidentTitle, result.Title);
        }
    }
}
