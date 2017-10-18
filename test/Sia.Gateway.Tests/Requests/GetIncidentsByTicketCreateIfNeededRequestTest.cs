using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain;
using Sia.Gateway.Requests;
using Sia.Gateway.ServiceRepositories;
using Sia.Gateway.Tests.TestDoubles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Requests
{
    [TestClass]
    public class GetIncidentsByTicketCreateIfNeededRequestTest
    {
        [TestMethod]
        public async Task Handle_WhenIncidentNotExist_ReturnNewIncident()
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
            var serviceUnderTest = new GetIncidentsByTicketCreateIfNeededRequestHandler(mockRepository);
            var request = new GetIncidentsByTicketCreateIfNeededRequest("100", new DummyAuthenticatedUserContext());


            var result = (await serviceUnderTest.Handle(request)).ToList();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual( "100", result[0].PrimaryTicket.OriginId);

        }
    }
}
