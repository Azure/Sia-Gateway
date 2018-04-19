using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Initialization;
using Sia.Gateway.Requests;
using Sia.Gateway.Tests.TestDoubles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Requests
{
    [TestClass]
    public class PostIncidentTests
    {
        [TestInitialize]
        public void ConfigureAutomapper()
            => AutoMapperStartup.InitializeAutomapper();

        [TestMethod]
        public async Task Handle_WhenIncidentClientReturnsSuccessful_ReturnCorrectIncidents()
        {
            string expectedIncidentTitle = "The thing we were looking for";
            var expectedIncident = new NewIncident
            {
                Title = expectedIncidentTitle,
                PrimaryTicket = new Ticket()
                {
                    OriginId = "testOnlyPleaseIgnore"
                }
            };

            var serviceUnderTest = new PostIncidentHandler(
                await MockFactory
                .IncidentContext(nameof(Handle_WhenIncidentClientReturnsSuccessful_ReturnCorrectIncidents))
                .ConfigureAwait(continueOnCapturedContext: false));
            var request = new PostIncidentRequest(expectedIncident, new DummyAuthenticatedUserContext());


            var result = await serviceUnderTest
                .Handle(request, new System.Threading.CancellationToken())
                .ConfigureAwait(continueOnCapturedContext: false);


            Assert.AreEqual(expectedIncidentTitle, result.Title);
        }
    }
}
