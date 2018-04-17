using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Core.Exceptions;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Initialization;
using Sia.Gateway.Requests;
using Sia.Gateway.Tests.TestDoubles;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Requests
{
    [TestClass]
    public class PostEngagementTests
    {
        [TestInitialize]
        public void ConfigureAutomapper()
            => AutoMapperStartup.InitializeAutomapper();

        [TestMethod]
        public async Task HandleWhenContextSavesEngagementReturnCorrectEngagementWithUpdateTime()
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
                ).ConfigureAwait(continueOnCapturedContext: false);
            var incident = context.Incidents.FirstOrDefault();
            var serviceUnderTest = new PostEngagementHandler(
               context
            );

            var request = new PostEngagementRequest(incident.Id, inputEngagement, new DummyAuthenticatedUserContext());

            var result = await serviceUnderTest
                .Handle(request, new System.Threading.CancellationToken())
                .ConfigureAwait(continueOnCapturedContext: false);

            Assert.AreEqual("test", result.Participant.Alias);
            Assert.AreEqual("alsoTest", result.Participant.Team);
            Assert.AreEqual("stillTest", result.Participant.Role);
            Assert.IsTrue(DateTime.UtcNow >= result.TimeEngaged);
            Assert.IsTrue(engagementTimeFloor <= result.TimeEngaged);
            Assert.IsNull(result.TimeDisengaged);
        }


        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task HandleWhenAssociatedIncidentDoesNotExistThrowKeyNotFoundException()
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

            var serviceUnderTest = new PostEngagementHandler(await MockFactory.IncidentContext(nameof(PostEngagementTests) /*+ nameof(Handle_WhenAssociatedIncidentDoesNotExist_ThrowKeyNotFoundException)*/).ConfigureAwait(continueOnCapturedContext: false));
            var request = new PostEngagementRequest(100_000, inputEngagement, new DummyAuthenticatedUserContext());

            var result = await serviceUnderTest
                .Handle(request, new System.Threading.CancellationToken())
                .ConfigureAwait(continueOnCapturedContext: false);

            //Expect exception
        }
    }
}
