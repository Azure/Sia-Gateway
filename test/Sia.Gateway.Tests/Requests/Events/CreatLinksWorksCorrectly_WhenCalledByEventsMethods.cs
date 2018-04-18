using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sia.Gateway.Controllers;
using Sia.Gateway.Tests.TestDoubles;
using Sia.Core.Protocol;
using System.Collections.Generic;

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
        
        string GetProperty(object values, string property) => values.GetType().GetProperty(property)?.GetValue(values).ToString() ?? "";

        [TestMethod]
        public void CreateLinksGeneratesFourLinksWithCorrectIds_WhenPassedAnIncidentIdAndAnEventId()
        {
            //Arrange
            methods.Clear();
            ids.Clear();
            var eventsController = new EventsController(null, null, null, urlHelperMock.Object, new StubLoggerFactory());
            // Act
            eventsController.CreateLinks("1", "2", null,null,"");

            // Assert
            urlHelperMock.Verify(foo => foo.Link(EventsController.GetSingleRouteName, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(EventsController.PostSingleRouteName, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(EventsController.GetMultipleRouteName, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(IncidentsController.GetSingleRouteName, It.IsAny<object>()), Times.Exactly(1));
            
            Assert.AreEqual(GetProperty(ids[0], "id"), "1");
            Assert.AreEqual(GetProperty(ids[1], "id"), "");
            Assert.AreEqual(GetProperty(ids[2], "id"), "");
            Assert.AreEqual(GetProperty(ids[3], "id"), "2");
        }

        [TestMethod]
        public void GetHeaderValuesAssignsMetadataAndPaginationAsNull_WhenNoMetaDataPassedIn()
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

            var linksHeaderWithoutMetadata = new LinksHeader(null, null, urlHelperMock.Object, "EventsController", null,
                null);
            var linksHeaderWithMetadata = new LinksHeader(null, pagination, urlHelperMock.Object, "EventsController", null,
                null);

            //Act
            var linksWithoutMetadata = linksHeaderWithoutMetadata.GetHeaderValues();
            var linksWithMetadata = linksHeaderWithMetadata.GetHeaderValues();

            //Assert
            Assert.IsNull(linksWithoutMetadata.Metadata);
            Assert.IsNull(linksWithoutMetadata.Links.Pagination);

            Assert.IsNotNull(linksWithMetadata.Metadata);
            Assert.IsNotNull(linksWithMetadata.Links.Pagination);
            urlHelperMock.Verify(foo => foo.Link("EventsController", It.IsAny<object>()), Times.Exactly(2));

            Assert.AreEqual(GetProperty(ids[0], "id"), "");
            Assert.AreEqual(GetProperty(ids[1], "id"), "");

        }
    }
}
