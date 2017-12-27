using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Gateway.Tests.TestDoubles
{
    public class StubLoggerFactory
        : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider) => throw new NotImplementedException();
        public ILogger CreateLogger(string categoryName) => null;
        public void Dispose() => throw new NotImplementedException();
    }
}
