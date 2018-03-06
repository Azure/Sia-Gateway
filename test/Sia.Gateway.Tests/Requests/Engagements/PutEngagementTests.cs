using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain.ApiModels;
using Sia.Gateway.Initialization;
using Sia.Gateway.Requests;
using Sia.Gateway.Tests.TestDoubles;
using Sia.Shared.Exceptions;
using System;
using System.Collections.Generic;
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
        public async Task Handle_WhenContextUpdatesEngagement_EngagementRecordInDatabaseReflectsUpdate()
        {
            var inputEngagement = new UpdateEngagement()
            {
                TimeDisengaged = new DateTime(1970, 10, 10)
            };

            var context = await MockFactory.IncidentContext(
                nameof(PutEngagementTests) 
                + nameof(Handle_WhenContextUpdatesEngagement_EngagementRecordInDatabaseReflectsUpdate)
            );

            var incident = context.Incidents.FirstOrDefault();
            var engagement = incident.Engagements.FirstOrDefault();
            var serviceUnderTest = new PutEngagementHandler(context);
            var request = new PutEngagementRequest(incident.Id, engagement.Id, inputEngagement, new DummyAuthenticatedUserContext());


            await serviceUnderTest.Handle(request, new System.Threading.CancellationToken());
            var result = context.Engagements.First(e => e.Id == engagement.Id);


            Assert.AreEqual(new DateTime(1970, 10, 10), result.TimeDisengaged);
        }


        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task Handle_WhenAssociatedIncidentDoesNotExist_ThrowKeyNotFoundException()
        {
            var inputEngagement = new UpdateEngagement()
            {
                TimeDisengaged = new DateTime(1070, 10, 10)
            };

            var context = await MockFactory.IncidentContext(nameof(PutEngagementTests) + nameof(Handle_WhenAssociatedIncidentDoesNotExist_ThrowKeyNotFoundException));

            var serviceUnderTest = new PutEngagementHandler(context);
            var request = new PutEngagementRequest(100_000, 1, inputEngagement, new DummyAuthenticatedUserContext());


            await serviceUnderTest.Handle(request, new System.Threading.CancellationToken());

            //Expect exception
        }
    }
}
