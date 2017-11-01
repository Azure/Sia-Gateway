using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Initialization;
using Sia.Gateway.Requests;
using Sia.Gateway.Tests.TestDoubles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Sia.Gateway.Tests.Requests
{
    [TestClass]
    public class PostEngagementTests
    {
        [TestInitialize]
        public void ConfigureAutomapper()
            => AutoMapperStartup.InitializeAutomapper();

        [TestMethod]
        public async Task Handle_WhenContextSavesEngagement_ReturnCorrectEngagementWithUpdateTime()
        {
            var engagementTimeFloor = DateTime.UtcNow;
            var inputEngagement = new NewEngagement()
            {
                Participant = new Participant()
                {
                    Alias = "test",
                    Team = "alsoTest",
                    Role = "stillTest"
                }
            };

            var context = await MockFactory.IncidentContext(
                    nameof(PostEngagementTests)
                    + "one"
                );
            var incident = context.Incidents.FirstOrDefault();
            var serviceUnderTest = new PostEngagementHandler(
               context
            );

            var request = new PostEngagementRequest(incident.Id, inputEngagement, new DummyAuthenticatedUserContext());

            var result = await serviceUnderTest.Handle(request);

            Assert.AreEqual("test", result.Participant.Alias);
            Assert.AreEqual("alsoTest", result.Participant.Team);
            Assert.AreEqual("stillTest", result.Participant.Role);
            Assert.IsTrue(DateTime.UtcNow >= result.TimeEngaged);
            Assert.IsTrue(engagementTimeFloor <= result.TimeEngaged);
            Assert.IsNull(result.TimeDisengaged);
        }


        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task Handle_WhenAssociatedIncidentDoesNotExist_ThrowKeyNotFoundException()
        {
            var engagementTimeFloor = DateTime.UtcNow;
            var inputEngagement = new NewEngagement()
            {
                Participant = new Participant()
                {
                    Alias = "test",
                    Team = "alsoTest",
                    Role = "stillTest"
                }
            };

            var serviceUnderTest = new PostEngagementHandler(await MockFactory.IncidentContext(nameof(PostEngagementTests) /*+ nameof(Handle_WhenAssociatedIncidentDoesNotExist_ThrowKeyNotFoundException)*/));
            var request = new PostEngagementRequest(100_000, inputEngagement, new DummyAuthenticatedUserContext());

            var result = await serviceUnderTest.Handle(request);

            //Expect exception
        }
    }
}
