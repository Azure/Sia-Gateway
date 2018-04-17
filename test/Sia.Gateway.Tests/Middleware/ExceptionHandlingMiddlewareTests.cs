using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Core.Middleware;
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
        public async Task InvokeWhenGatewayExceptionThrownErrorWrittenToResponse()
        {
            var objectUnderTest = new ExceptionHandler(ThrowFakeGatewayException);
            var inputContext = new StubHttpContext();

            await objectUnderTest.Invoke(inputContext).ConfigureAwait(continueOnCapturedContext: false);

            Assert.AreEqual(FakeGatewayStatusCode, inputContext.Response.StatusCode);
            Assert.AreEqual("{\"error\":\"Test Gateway Exception\"}", ((StubHttpResponse)inputContext.Response).ReadBody());
            Assert.AreEqual("application/json", inputContext.Response.ContentType);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task InvokeWhenNonGatewayExceptionThrownExceptionNotCaught()
        {
            var objectUnderTest = new ExceptionHandler(ThrowException);
            var inputContext = new StubHttpContext();

            await objectUnderTest.Invoke(inputContext).ConfigureAwait(continueOnCapturedContext: false);

            //Expect exception
        }

        [TestMethod]
        public async Task InvokeWhenNoExceptionThrownMiddlewareTakesNoAction()
        {
            var objectUnderTest = new ExceptionHandler(DoNothing);
            var inputContext = new StubHttpContext();

            await objectUnderTest.Invoke(inputContext).ConfigureAwait(continueOnCapturedContext: false);

            //No exception thrown
        }

        public static Task ThrowFakeGatewayException(HttpContext context)
            => throw new FakeGatewayException(FakeGatewayExceptionMessage, FakeGatewayStatusCode);

        public static Task ThrowException(HttpContext context)
            => throw new Exception("IGNORE ME");

        public static Task DoNothing(HttpContext context)
            => Task.CompletedTask;
    }


}
