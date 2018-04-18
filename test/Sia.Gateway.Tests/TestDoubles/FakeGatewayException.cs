using Sia.Core.Exceptions;

namespace Sia.Gateway.Tests.Middleware
{
#pragma warning disable CA1032 // Implement standard exception constructors
    public class FakeGatewayException : BaseException
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        private int _expectedStatusCode;

        public FakeGatewayException(string message, int expectedStatusCode) : base(message)
        {
            _expectedStatusCode = expectedStatusCode;
        }

        public override int StatusCode => _expectedStatusCode;
    }
}
