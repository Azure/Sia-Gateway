﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sia.Gateway.Controllers;
using Sia.Shared.Protocol;
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

        [TestMethod]
        public void CreateLinksWorksCorrectly_WhenCalledInEventsMethods()
        {
            //Arrange
            methods.Clear();
            ids.Clear();
            var eventsController = new EventsController(null, null, null, urlHelperMock.Object);
            // Act
            eventsController.CreateLinks("1", "2");
            
            // Assert
            urlHelperMock.Verify(foo => foo.Link(EventsController.GetSingleRouteName,  It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(EventsController.PostSingleRouteName, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(EventsController.GetMultipleRouteName, It.IsAny<object>()), Times.Exactly(1));
            urlHelperMock.Verify(foo => foo.Link(IncidentsController.GetSingleRouteName, It.IsAny<object>()), Times.Exactly(1));

            Assert.AreEqual(ids[0].ToString(), "{ id = 1 }");
            Assert.AreEqual(ids[1].ToString(), "{ }");
            Assert.AreEqual(ids[2].ToString(), "{ }");
            Assert.AreEqual(ids[3].ToString(), "{ id = 2 }");

        }

        [TestMethod]
        public void GetHeaderValuesWorksCorrectly_WhenCalledInEventsMethods()
        {
            //Arrange
            methods.Clear();
            ids.Clear();
            var linksHeader = new LinksHeader(new PaginationMetadata(), urlHelperMock.Object, "EventsController", null,
                null);
            //Act
            var linksForSerialization = linksHeader.GetHeaderValues();

            //Assert
            Assert.IsNotNull(linksForSerialization.Metadata);
            //Assert.IsNotNull(linksForSerialization.Links.Pagination);
        }
    }
}
