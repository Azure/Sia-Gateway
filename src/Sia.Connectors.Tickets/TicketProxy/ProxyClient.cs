using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sia.Connectors.Tickets.TicketProxy
{
    public class ProxyClient : Client<Ticket>
    {
        private readonly string _endpoint;
        private readonly HttpClient _client;

        public ProxyClient(HttpClient singletonClient, string endpoint)
        {
            _endpoint = endpoint;
            _client = singletonClient;
        }

        public override async Task<Ticket> GetAsync(string originId)
        {
            string incidentUrl = $"{_endpoint}/{originId}";
            var response = await _client.GetAsync(incidentUrl);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Ticket>(content);
        }
    }
}
