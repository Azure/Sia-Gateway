using Sia.Shared.Transactions;
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

        public object Value => throw new NotImplementedException();
    }
}
