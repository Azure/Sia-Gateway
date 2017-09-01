using Sia.Shared.Transactions;
using System.Net;

namespace Sia.Gateway.Tests.TestDoubles
{
    public class StubResponse<T>
        : IResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccessStatusCode { get; set; }

        public string Content { get; set; }

        public T Value { get; set; }
    }
}
