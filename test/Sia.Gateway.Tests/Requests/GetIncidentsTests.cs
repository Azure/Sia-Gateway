using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain;
using Sia.Gateway.Requests;
using Sia.Gateway.ServiceRepositories;
using Sia.Gateway.Tests.TestDoubles;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Requests
{

    [TestClass]
    public class GetIncidentsTests
    {
        [TestMethod]
        public async Task Handle_WhenIncidentClientReturnsSuccessful_ReturnCorrectIncidents()
        {
            long[] expectedIncidentIds = { 200, 300, 400 };
            string[] expectedIncidentTitles = { "First", "Second", "Third" };
            Incident[] expectedIncidents = new Incident[expectedIncidentIds.Length];
            for (int i = 0; i < expectedIncidents.Length; i++)
            {
                expectedIncidents[i] = new Incident
                {
                    Id = expectedIncidentIds[i],
                    Title = expectedIncidentTitles[i]
                };
            }
            IIncidentRepository mockRepository = new StubIncidentRepository(expectedIncidents, null);
            var serviceUnderTest = new GetIncidentsHandler(mockRepository);
            var request = new GetIncidentsRequest(new DummyAuthenticatedUserContext());


            var result = (await serviceUnderTest.Handle(request)).ToList();


            for (int i = 0; i < expectedIncidents.Length; i++)
            {
                Assert.AreEqual(expectedIncidentIds[i], result[i].Id);
                Assert.AreEqual(expectedIncidentTitles[i], result[i].Title);
            } 
        }
    }
}
