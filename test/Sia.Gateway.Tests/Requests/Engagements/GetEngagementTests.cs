﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain;
using Sia.Gateway.Initialization;
using Sia.Gateway.Requests;
using Sia.Gateway.Tests.TestDoubles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Requests
{
    [TestClass]
    public class GetEngagementTests
    {
        [TestInitialize]
        public void ConfigureAutomapper()
            => AutoMapperStartup.InitializeAutomapper();

        [TestMethod]
        public async Task Handle_WhenEFReturnsSuccessful_ReturnCorrectEngagement()
        {
            var expectedEngagement = new Engagement
            {
                Id = 1,
                IncidentId = 1,
                TimeEngaged = new DateTime(1973, 3, 3),
                TimeDisengaged = new DateTime(1974, 4, 4),
                Participant = new Participant
                {
                    Alias = "pdimit",
                    Team = "Sia Engineering",
                    Role = "Crisis Manager"
                }
            };

            var serviceUnderTest = new GetEngagementHandler(await MockFactory.IncidentContext(nameof(Handle_WhenEFReturnsSuccessful_ReturnCorrectEngagement)));
            var request = new GetEngagementRequest(1, 1, new DummyAuthenticatedUserContext());


            var result = await serviceUnderTest.Handle(request, new System.Threading.CancellationToken());


            Assert.AreEqual(expectedEngagement.Id, result.Id);
            Assert.AreEqual(expectedEngagement.TimeDisengaged, result.TimeDisengaged);
            Assert.AreEqual(expectedEngagement.TimeEngaged, result.TimeEngaged);
            Assert.AreEqual(expectedEngagement.Participant.Alias, result.Participant.Alias);
        }


        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task Handle_WhenRecordDoesNotExistInEF_ThrowKeyNotFoundException()
        {
            var serviceUnderTest = new GetEngagementHandler(await MockFactory.IncidentContext("Get"));
            var request = new GetEngagementRequest(100_000, 1, new DummyAuthenticatedUserContext());


            var result = await serviceUnderTest.Handle(request, new System.Threading.CancellationToken());


            //Expect exception
        }
    }
}
