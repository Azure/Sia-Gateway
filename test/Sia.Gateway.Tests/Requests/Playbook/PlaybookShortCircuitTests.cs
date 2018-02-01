using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain.Playbook;
using Sia.Gateway.Requests;
using Sia.Gateway.Tests.TestDoubles;
using Sia.Shared.Authentication;
using Sia.Shared.Extensions.Mediatr;

namespace Sia.Gateway.Tests.Requests.Playbook
{
    [TestClass]
    public class PlaybookShortCircuitTests
    {
        public Task<EventType> ReturnNull()
        {
            return Task.FromResult(new EventType()
            {
                Id = 42,
                Name = "This is a mock",
                Data = new MockEventTypeData()
            });
        }

        [TestMethod]
        public void ShouldRequestContinue__Returns_True_If_Playbook_In_Config()
        {
            var configProps = new Dictionary<string, string>();
            configProps.Add("Microservices:0", "Playbook");
            var mockConfig = new MockConfig(configProps);
            var getEventTypesShortCircuit = new GetEventTypeShortCircuit(mockConfig);
            var result = getEventTypesShortCircuit.ShouldRequestContinue(mockConfig);

            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void Handle_Should_Return_Next_If_Playbook_In_Config()
        {
            var configProps = new Dictionary<string, string>();
            configProps.Add("Microservices:0", "Playbook");
            var mockConfig = new MockConfig(configProps);
            var getEventTypesShortCircuit = new GetEventTypeShortCircuit(mockConfig);
            var mockContext = new DummyAuthenticatedUserContext();
            var request = new GetEventTypeRequest(10000000001, mockContext);
            var mockEventType = new EventType();
            mockEventType.Id = 42;
            var mockDelegate = new RequestHandlerDelegate<EventType>(ReturnNull);

            var result = getEventTypesShortCircuit.Handle(request, new CancellationToken(), mockDelegate);

            Assert.AreEqual(result.Id, mockEventType.Id);
        }
    }
}
 