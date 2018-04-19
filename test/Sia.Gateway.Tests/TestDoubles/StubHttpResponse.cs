using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Middleware
{
    #pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
    public class StubHttpResponse : HttpResponse
    {
        private HttpContext _context;

        public StubHttpResponse(HttpContext context)
        {
            _context = context;
        }

        public string ReadBody()
        {
            _body.Seek(0, SeekOrigin.Begin);
            var bodyReader = new StreamReader(_body);
            return bodyReader.ReadToEnd();
        }

        private Stream _body { get; set; }
            = new MemoryStream();

        public override HttpContext HttpContext => _context;

        public override int StatusCode { get; set; }

        public override IHeaderDictionary Headers => throw new NotImplementedException();

        public override Stream Body { get => _body; set => _body = value; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }

        public override IResponseCookies Cookies => throw new NotImplementedException();

        public override bool HasStarted => throw new NotImplementedException();

        public override void OnCompleted(Func<object, Task> callback, object state)
        {
            
        }

        public override void OnStarting(Func<object, Task> callback, object state)
        {
            
        }

        public override void Redirect(string location, bool permanent)
        {
            throw new NotImplementedException();
        }
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
    }
}
