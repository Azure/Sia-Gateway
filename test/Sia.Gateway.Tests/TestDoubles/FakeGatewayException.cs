﻿using Sia.Core.Exceptions;

namespace Sia.Gateway.Tests.Middleware
{
    public class FakeGatewayException : BaseException
    {
        private int _expectedStatusCode;

        public FakeGatewayException(string message, int expectedStatusCode) : base(message)
        {
            _expectedStatusCode = expectedStatusCode;
        }

        public override int StatusCode => _expectedStatusCode;
    }
}
