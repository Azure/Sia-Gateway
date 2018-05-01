using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sia.Gateway.Controllers;
using Sia.Gateway.Tests.TestDoubles;
using Sia.Core.Protocol;
using System.Collections.Generic;
using Sia.Gateway.Links;

namespace Sia.Gateway.Tests.Requests.Events
{
    [TestClass]
    public class GenerateLinksHeaderTestsForEvents
    {
        private List<string> methods;
        private List<object> ids;
        private Mock<IUrlHelper> urlHelperMock;
        public GenerateLinksHeaderTestsForEvents()
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
        public void CreateLinks_GeneratesFourLinksWithCorrectIds_WhenPassedAnIncidentIdAndAnEventId()
        {
            //Arrange
            methods.Clear();
            ids.Clear();
            var eventLinksProvider = new EventLinksProvider(urlHelperMock.Object);
            // Act
            eventLinksProvider.CreateLinks(2, 1);

            // Assert
            urlHelperMock.Verify(foo => foo.Link(EventRoutesByIncident.GetSingle, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(EventRoutesByIncident.PostSingle, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(EventRoutesByIncident.GetMultiple, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(IncidentRoutes.GetSingle, It.IsAny<object>()), Times.Exactly(1));
            
            Assert.AreEqual(GetProperty(ids[0], "eventId"), "1");
            Assert.AreEqual(GetProperty(ids[1], "eventId"), "1");
            Assert.AreEqual(GetProperty(ids[2], "eventId"), "1");
            Assert.AreEqual(GetProperty(ids[3], "incidentId"), "2");
        }
    }
}
