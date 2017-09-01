using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Gateway.Middleware;
using System;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Middleware
{
    [TestClass]
    public class ExceptionHandlingMiddlewareTests
    {
        const string FakeGatewayExceptionMessage = "Test Gateway Exception";
        const int FakeGatewayStatusCode = 555;
        [TestMethod]
        public async Task Invoke_WhenGatewayExceptionThrown_ErrorWrittenToResponse()
        {
            var objectUnderTest = new ExceptionHandler(ThrowFakeGatewayException);
            var inputContext = new StubHttpContext();

            await objectUnderTest.Invoke(inputContext);

            Assert.AreEqual(FakeGatewayStatusCode, inputContext.Response.StatusCode);
            Assert.AreEqual("{\"error\":\"Test Gateway Exception\"}", ((StubHttpResponse)inputContext.Response).ReadBody());
            Assert.AreEqual("application/json", inputContext.Response.ContentType);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task Invoke_WhenNonGatewayExceptionThrown_ExceptionNotCaught()
        {
            var objectUnderTest = new ExceptionHandler(ThrowException);
            var inputContext = new StubHttpContext();

            await objectUnderTest.Invoke(inputContext);

            //Expect exception
        }

        [TestMethod]
        public async Task Invoke_WhenNoExceptionThrown_MiddlewareTakesNoAction()
        {
            var objectUnderTest = new ExceptionHandler(DoNothing);
            var inputContext = new StubHttpContext();

            await objectUnderTest.Invoke(inputContext);

            //No exception thrown
        }

        public static Task ThrowFakeGatewayException(HttpContext context)
        {
            throw new FakeGatewayException(FakeGatewayExceptionMessage, FakeGatewayStatusCode);
        }

        public static Task ThrowException(HttpContext context)
        {
            throw new Exception("IGNORE ME");
        }

        public static Task DoNothing(HttpContext context)
        {
            return Task.CompletedTask;
        }
    }


}
