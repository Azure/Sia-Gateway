using Sia.Shared.Exceptions;

namespace Sia.Gateway.Tests.Middleware
{
    public class FakeGatewayException : GatewayException
    {
        private int _expectedStatusCode;

        public FakeGatewayException(string message, int expectedStatusCode) : base(message)
        {
            _expectedStatusCode = expectedStatusCode;
        }

        public override int StatusCode => _expectedStatusCode;
    }
}
