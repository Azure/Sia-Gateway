using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Sia.Gateway.Tests.TestDoubles
{
    public class DummySession : ISession
    {
        // Disabling warnings related to NotImplementedException in a stub class
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
        public bool IsAvailable => throw new NotImplementedException();

        public string Id => throw new NotImplementedException();

        public IEnumerable<string> Keys => throw new NotImplementedException();

        public void Clear() => throw new NotImplementedException();

        public Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
            => throw new NotImplementedException();

        public Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
            => throw new NotImplementedException();

        public void Remove(string key) => throw new NotImplementedException();

        public void Set(string key, byte[] value) => throw new NotImplementedException();

        public bool TryGetValue(string key, out byte[] value) => throw new NotImplementedException();
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
    }
}
