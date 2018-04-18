using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Connectors.Tickets;
using Sia.Gateway.Initialization;
using Sia.Gateway.Tests.TestDoubles;

namespace Sia.Gateway.Tests
{

    [TestClass]
    public class InitializationTests
    {
        [TestMethod]
        public void AddTicketingConnectorReturnsServiceWithNoTicketingSystemWhenConnectorConfigNull()
        {
            var mockServices = new SpyServiceCollection();
            var mockEnv = new Mock<IHostingEnvironment>();
            var mockConfig = new Mock<IConfigurationRoot>();

            var result = mockServices.AddTicketingConnector(mockEnv.Object, 
            mockConfig.Object, 
            null);
            var finalResult = result.ToArray();

            Assert.AreEqual(finalResult[0].ImplementationType.Name, "NoClient");
            Assert.AreEqual(finalResult[1].ImplementationType.Name, "NoConnector");
        }

        [TestMethod]
        public void AddTicketingConnectorReturnsServiceWithNoTicketingSystemWhenConnectorConfigPathIsWhiteSpace()
        {
            var mockServices = new SpyServiceCollection();
            var mockEnv = new Mock<IHostingEnvironment>();
            var mockConfig = new Mock<IConfigurationRoot>();
            var mockConnectorConfig = new TicketingConnectorConfig()
            {
                Path = ""
            };

            var result = mockServices.AddTicketingConnector(mockEnv.Object,
                mockConfig.Object,
                mockConnectorConfig);
            var finalResult = result.ToArray();

            Assert.AreEqual(finalResult[0].ImplementationType.Name, "NoClient");
            Assert.AreEqual(finalResult[1].ImplementationType.Name, "NoConnector");
        }
    }
}
