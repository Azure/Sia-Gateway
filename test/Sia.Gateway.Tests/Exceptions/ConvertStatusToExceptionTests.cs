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
        public void ThrowExceptionOnUnsuccessfulStatusWhenResponseIsSuccessfulDoesNotThrowException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.Accepted, true);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //No exception
        }

        [TestMethod]
        [ExpectedException(typeof(BadRequestException))]
        public void ThrowExceptionOnUnsuccessfulStatusWhenUnsuccessfulAndBadRequestThrowBadRequestException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.BadRequest);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //Expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(ConflictException))]
        public void ThrowExceptionOnUnsuccessfulStatusWhenUnsuccessfulAndConflictThrowConflictException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.Conflict);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //Expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedException))]
        public void ThrowExceptionOnUnsuccessfulStatusWhenUnsuccessfulAndForbiddenThrowUnauthorizedException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.Forbidden);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //Expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedException))]
        public void ThrowExceptionOnUnsuccessfulStatusWhenUnsuccessfulAndUnauthorizedThrowUnauthorizedException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.Unauthorized);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //Expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public void ThrowExceptionOnUnsuccessfulStatusWhenUnsuccessfulAndNotFoundThrowNotFoundException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.NotFound);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //Expect exception
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ThrowExceptionOnUnsuccessfulStatusWhenUnsuccessfulAndUnsupportedStatusCodeThrowException()
        {
            var testInput = new FakeLiveSiteResponse(HttpStatusCode.NotAcceptable);

            testInput.ThrowExceptionOnUnsuccessfulStatus();

            //Expect exception
        }
    }
}
