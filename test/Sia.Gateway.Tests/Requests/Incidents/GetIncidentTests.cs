﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Connectors.Tickets.None;
using Sia.Domain;
using Sia.Gateway.Initialization;
using Sia.Gateway.Requests;
using Sia.Gateway.Tests.TestDoubles;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Requests
{
    [TestClass]
    public class GetIncidentTests
    {
        [TestInitialize]
        public void ConfigureAutomapper()
            => AutoMapperStartup.InitializeAutomapper();
        [TestMethod]
        public async Task HandleWhenIncidentClientReturnsSuccessfulReturnCorrectIncident()
        {
            long expectedIncidentId = 1;
            string expectedIncidentTitle = "Customers are unable to access [REDACTED] from [REDACTED]";
            var expectedIncident = new Incident
            {
                Id = expectedIncidentId,
                Title = expectedIncidentTitle
            };
            var serviceUnderTest = new GetIncidentHandler(
                await MockFactory.IncidentContext("Get").ConfigureAwait(continueOnCapturedContext: false),
                new NoConnector(new NoClient(), new StubLoggerFactory()));
            var request = new GetIncidentRequest(
                expectedIncidentId,
                new DummyAuthenticatedUserContext());


            var result = await serviceUnderTest
                .Handle(request, new System.Threading.CancellationToken())
                .ConfigureAwait(continueOnCapturedContext: false);


            Assert.AreEqual(expectedIncidentId, result.Id);
            Assert.AreEqual(expectedIncidentTitle, result.Title);
        }
    }
}
