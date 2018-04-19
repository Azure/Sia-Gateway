using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Core.Exceptions;
using System;
using System.Net;

namespace Sia.Gateway.Tests.Exceptions
{
    [TestClass]
    public class ConvertStatusToExceptionTests
    {
        [TestMethod]
        public void ThrowExceptionOnUnsuccessfulStatus_WhenResponseIsSuccessful_DoesNotThrowException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.Accepted, true);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //No exception
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public void ThrowExceptionOnUnsuccessfulStatus_WhenUnsuccessfulAndBadRequest_ThrowBadRequestException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.BadRequest);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //Expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(ConflictException))]
        public void ThrowExceptionOnUnsuccessfulStatus_WhenUnsuccessfulAndConflict_ThrowConflictException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.Conflict);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //Expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedException))]
        public void ThrowExceptionOnUnsuccessfulStatus_WhenUnsuccessfulAndForbidden_ThrowUnauthorizedException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.Forbidden);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //Expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedException))]
        public void ThrowExceptionOnUnsuccessfulStatus_WhenUnsuccessfulAndUnauthorized_ThrowUnauthorizedException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.Unauthorized);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //Expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void ThrowExceptionOnUnsuccessfulStatus_WhenUnsuccessfulAndNotFound_ThrowNotFoundException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.NotFound);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //Expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ThrowExceptionOnUnsuccessfulStatus_WhenUnsuccessfulAndUnsupportedStatusCode_ThrowException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.NotAcceptable);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //Expect exception
        }
    }
}
