using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sia.Gateway.Controllers;
using Sia.Core.Protocol;
using System.Collections.Generic;
using Sia.Gateway.Links;

namespace Sia.Gateway.Tests.Requests.Incidents
{
    [TestClass]
    public class GenerateLinksHeaderTestsForIncidents
    {
        private List<string> methods;
        private List<object> ids;
        private Mock<IUrlHelper> urlHelperMock;
        public GenerateLinksHeaderTestsForIncidents()
        {
            // Arrange
            methods = new List<string>();
            ids = new List<object>();
            urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(link => link.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Callback<string, object>(
                    (s, o) =>
                    {
                        methods.Add(s);
                        ids.Add(o);
                    }
                );

        }

        static string GetProperty(object values, string property) => values.GetType().GetProperty(property)?.GetValue(values).ToString() ?? "";

        [TestMethod]
        public void CreateLinks_GeneratesFourLinksWithCorrectIdsOrIncidentIds_WhenPassedAnIncidentId()
        {
            // Arrange
            var methods = new List<string>();
            var ids = new List<object>();
            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(link => link.Link(It.IsAny<string>(), It.IsAny<object>()))
                .Callback<string, object>(
                    (s, o) =>
                    {
                        methods.Add(s);
                        ids.Add(o);
                    }
                );
            var incidentLinksProvider = new IncidentLinksProvider(urlHelperMock.Object);

            // Act
            incidentLinksProvider.CreateLinks(1);

            // Assert
            urlHelperMock.Verify(foo => foo.Link(IncidentRoutes.GetSingle, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(IncidentRoutes.PostSingle, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(IncidentRoutes.GetMultiple, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(EventRoutesByIncident.GetMultiple, It.IsAny<object>()), Times.Exactly(1));
            
            Assert.AreEqual(GetProperty(ids[0], "incidentId"), "1");
            Assert.AreEqual(GetProperty(ids[1], "incidentId"), "1");
            Assert.AreEqual(GetProperty(ids[2], "incidentId"), "1");
            Assert.AreEqual(GetProperty(ids[3], "incidentId"), "1");
        }
    }
}
