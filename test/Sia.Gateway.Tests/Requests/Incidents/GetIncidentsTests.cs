﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Connectors.Tickets.None;
using Sia.Domain;
using Sia.Gateway.Initialization;
using Sia.Gateway.Requests;
using Sia.Gateway.Tests.TestDoubles;
using System.Linq;
using System.Threading.Tasks;
using Sia.Core.Protocol;

namespace Sia.Gateway.Tests.Requests
{

    [TestClass]
    public class GetIncidentsTests
    {
        [TestInitialize]
        public void ConfigureAutomapper()
            => AutoMapperStartup.InitializeAutomapper();

        [TestMethod]
        public async Task Handle_WhenIncidentClientReturnsSuccessful_ReturnCorrectIncidents()
        {
            long[] expectedIncidentIds = { 1, 2, 3 };
            string[] expectedIncidentTitles = {
                "Customers are unable to access [REDACTED] from [REDACTED]",
                "Loss of [REDACTED] Connectivity in [REDACTED]",
                "[REDACTED] and [REDACTED] service management operations for a subset of users in [REDACTED] are failing"
            };
            Incident[] expectedIncidents = new Incident[expectedIncidentIds.Length];
            for (int i = 0; i < expectedIncidents.Length; i++)
            {
                expectedIncidents[i] = new Incident
                {
                    Id = expectedIncidentIds[i],
                    Title = expectedIncidentTitles[i]
                };
            }
            var serviceUnderTest = new GetIncidentsHandler(
                await MockFactory.IncidentContext(nameof(Handle_WhenIncidentClientReturnsSuccessful_ReturnCorrectIncidents)),
                new NoConnector(new NoClient(), new StubLoggerFactory())
            );
            var request = new GetIncidentsRequest(new PaginationMetadata(), new DummyAuthenticatedUserContext());


            var result = (await serviceUnderTest.Handle(request, new System.Threading.CancellationToken())).ToList();


            for (int i = 0; i < expectedIncidents.Length; i++)
            {
                Assert.AreEqual(expectedIncidentIds[i], result[i].Id);
                Assert.AreEqual(expectedIncidentTitles[i], result[i].Title);
            } 
        }
    }
}
