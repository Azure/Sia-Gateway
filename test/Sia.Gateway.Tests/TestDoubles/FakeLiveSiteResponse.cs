using Sia.Core.Protocol;
using System;
using System.Net;

namespace Sia.Gateway.Tests.Exceptions
{
    public class FakeLiveSiteResponse : IResponse<object>
    {
        private HttpStatusCode _expectedStatusCode;
        private bool _isSuccessful;

        public FakeLiveSiteResponse(HttpStatusCode expectedStatusCode, bool isSuccessful = false)
        {
            _expectedStatusCode = expectedStatusCode;
            _isSuccessful = isSuccessful;
        }
        public HttpStatusCode StatusCode => _expectedStatusCode;

        public bool IsSuccessStatusCode => _isSuccessful;

        public string Content => "Fake Content Value";

        // Disabling warnings related to NotImplementedException in a stub class
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
        public object Value => throw new NotImplementedException();
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
    }
}
