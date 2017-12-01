using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sia.Shared.Authentication;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyClient : TicketingClient
    {
        private ProxyConnectionInfo _connectionInfo { get; }
        private HttpClient _client { get; }

        public ProxyClient(
            ProxyConnectionInfo connectionInfo,
            ILoggerFactory loggerFactory
        )
        {
            _connectionInfo = connectionInfo;
            _client = _connectionInfo.GetClient(loggerFactory);
        }

        public override async Task<object> GetAsync(string originId)
        {
            string incidentUrl = $"{_connectionInfo.Endpoint}/{originId}";
            var response = await _client.GetAsync(incidentUrl);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProxyData>(content);
        }


    }
}
