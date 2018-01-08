using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Connectors.Tickets.None;
using Sia.Domain;
using Sia.Gateway.Initialization;
using Sia.Gateway.Requests;
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
        [TestInitialize]
        public void ConfigureAutomapper()
    => AutoMapperStartup.InitializeAutomapper();

        [TestMethod]
        public async Task Handle_WhenIncidentNotExist_ReturnNewIncident()
        {
            var serviceUnderTest = new GetIncidentsByTicketCreateIfNeededRequestHandler(
                await MockFactory.IncidentContext("Get"),
                new NoConnector(new NoClient(), new StubLoggerFactory())
            );
            var request = new GetIncidentsByTicketCreateIfNeededRequest("100", new DummyAuthenticatedUserContext());


            var result = (await serviceUnderTest.Handle(request, new System.Threading.CancellationToken())).ToList();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual( "100", result[0].PrimaryTicket.OriginId);

        }

        [TestMethod]
        public async Task Handle_WhenIncidentExists_ReturnCorrectIncidents()
        {

            var serviceUnderTest = new GetIncidentsByTicketCreateIfNeededRequestHandler(
                await MockFactory.IncidentContext(nameof(Handle_WhenIncidentExists_ReturnCorrectIncidents)),
                new NoConnector(new NoClient(), new StubLoggerFactory())
            );
            var request = new GetIncidentsByTicketCreateIfNeededRequest("44444444", new DummyAuthenticatedUserContext());

            var result = (await serviceUnderTest.Handle(request, new System.Threading.CancellationToken())).ToList();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result[0].Id);

        }
    }
}
