using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;

namespace Sia.Gateway.Tests.Middleware
{
    public class StubHttpContext : HttpContext
    {
        public StubHttpContext()
        {
            _response = new StubHttpResponse(this);
        }


        private HttpResponse _response { get; set; }

        // Disabling warnings related to NotImplementedException in a stub class
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations

        public override IFeatureCollection Features => throw new NotImplementedException();

        public override HttpRequest Request => throw new NotImplementedException();

        public override HttpResponse Response => _response;

        public override ConnectionInfo Connection => throw new NotImplementedException();

        public override WebSocketManager WebSockets => throw new NotImplementedException();

        public override ClaimsPrincipal User { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IServiceProvider RequestServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override CancellationToken RequestAborted { get; set; }
        public override string TraceIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [Obsolete("Will be removed in later version")]
        public override AuthenticationManager Authentication => throw new NotImplementedException();

        public override void Abort() => throw new NotImplementedException();
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
    }
}
