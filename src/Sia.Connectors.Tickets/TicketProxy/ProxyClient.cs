using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sia.Core.Authentication;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyClient : TicketingClient
    {
        private readonly ILoggerFactory _loggerFactory;

        private ProxyConnectionInfo _connectionInfo { get; }
        private HttpClient _client { get; set; }

        public ProxyClient(
            ProxyConnectionInfo connectionInfo,
            ILoggerFactory loggerFactory
        )
        {
            _loggerFactory = loggerFactory;
            _connectionInfo = connectionInfo;
        }

        public override async Task<object> GetAsync(string originId)
        {
            if(_client is null)
            {
                _client = await _connectionInfo.GetClientAsync(_loggerFactory).ConfigureAwait(continueOnCapturedContext: false);
            }
            var incidentUrl = new Uri($"{_connectionInfo.Endpoint}/{originId}");
            var response = await _client.GetAsync(incidentUrl).ConfigureAwait(continueOnCapturedContext: false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
            return JsonConvert.DeserializeObject<ProxyData>(content);
        }


    }
}
