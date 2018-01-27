using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sia.Gateway.Controllers;
using Sia.Shared.Protocol;
using System.Collections.Generic;


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
        [TestMethod]
        public void CreateLinksWorksCorrectly_WhenCalledInIncidentsMethods()
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
            var incidentsController = new IncidentsController(null, null, urlHelperMock.Object);

            // Act
            incidentsController.CreateLinks("1", null, "");

            // Assert
            urlHelperMock.Verify(foo => foo.Link(IncidentsController.GetSingleRouteName, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(IncidentsController.PostSingleRouteName, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(IncidentsController.GetMultipleRouteName, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(EventsController.GetMultipleRouteName, It.IsAny<object>()), Times.Exactly(1));

            Assert.AreEqual(ids[0].ToString(), "{ id = 1 }");
            Assert.AreEqual(ids[1].ToString(), "{ }");
            Assert.AreEqual(ids[2].ToString(), "{ }");
            Assert.AreEqual(ids[3].ToString(), "{ incidentId = 1 }");

        }

        [TestMethod]
        public void GetHeaderValuesWorksCorrectly_WhenCalledInIncidentsMethods()
        {
            //Arrange
            methods.Clear();
            ids.Clear();

            var pagination = new PaginationMetadata()
            {
                PageNumber = 2,
                PageSize = 2,
                TotalRecords = 10

            };

            var linksHeaderWithoutMetadata = new LinksHeader(null, null, urlHelperMock.Object, "IncidentsController", null,
                null);
            var linksHeaderWithMetadata = new LinksHeader(null, pagination, urlHelperMock.Object, "IncidentsController", null,
                null);

            //Act
            var linksWithoutMetadata = linksHeaderWithoutMetadata.GetHeaderValues();
            var linksWithMetadata = linksHeaderWithMetadata.GetHeaderValues();

            //Assert
            Assert.IsNull(linksWithoutMetadata.Metadata);
            Assert.IsNull(linksWithoutMetadata.Links.Pagination);

            Assert.IsNotNull(linksWithMetadata.Metadata);
            Assert.IsNotNull(linksWithMetadata.Links.Pagination);
            urlHelperMock.Verify(foo => foo.Link("IncidentsController", It.IsAny<object>()), Times.Exactly(2));

            Assert.AreEqual(ids[0].ToString(), "{ }");
            Assert.AreEqual(ids[1].ToString(), "{ }");
        }
    }
}
