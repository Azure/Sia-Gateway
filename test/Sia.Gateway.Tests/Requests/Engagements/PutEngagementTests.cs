using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Core.Exceptions;
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
    public class PutEngagementTests
    {
        [TestInitialize]
        public void ConfigureAutomapper()
            => AutoMapperStartup.InitializeAutomapper();

        [TestMethod]
        public async Task HandleWhenContextUpdatesEngagementEngagementRecordInDatabaseReflectsUpdate()
        {
            var inputEngagement = new UpdateEngagement()
            {
                TimeDisengaged = new DateTime(1970, 10, 10)
            };

            var context = await MockFactory.IncidentContext(
                nameof(PutEngagementTests) 
                + nameof(HandleWhenContextUpdatesEngagementEngagementRecordInDatabaseReflectsUpdate)
            ).ConfigureAwait(continueOnCapturedContext: false);

            var incident = context.Incidents.FirstOrDefault();
            var engagement = incident.Engagements.FirstOrDefault();
            var serviceUnderTest = new PutEngagementHandler(context);
            var request = new PutEngagementRequest(incident.Id, engagement.Id, inputEngagement, new DummyAuthenticatedUserContext());


            await serviceUnderTest
                .Handle(request, new System.Threading.CancellationToken())
                .ConfigureAwait(continueOnCapturedContext: false);
            var result = context.Engagements.First(e => e.Id == engagement.Id);


            Assert.AreEqual(new DateTime(1970, 10, 10), result.TimeDisengaged);
        }


        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task HandleWhenAssociatedIncidentDoesNotExistThrowKeyNotFoundException()
        {
            var inputEngagement = new UpdateEngagement()
            {
                TimeDisengaged = new DateTime(1070, 10, 10)
            };

            var context = await MockFactory
                .IncidentContext(nameof(PutEngagementTests) + nameof(HandleWhenAssociatedIncidentDoesNotExistThrowKeyNotFoundException))
                .ConfigureAwait(continueOnCapturedContext: false);

            var serviceUnderTest = new PutEngagementHandler(context);
            var request = new PutEngagementRequest(100_000, 1, inputEngagement, new DummyAuthenticatedUserContext());


            await serviceUnderTest
                .Handle(request, new System.Threading.CancellationToken())
                .ConfigureAwait(continueOnCapturedContext: false);

            //Expect exception
        }
    }
}
