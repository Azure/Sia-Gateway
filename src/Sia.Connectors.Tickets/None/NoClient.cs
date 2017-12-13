using System.Threading.Tasks;

namespace Sia.Connectors.Tickets.None
{
    public class NoClient : TicketingClient
    {
        public override Task<object> GetAsync(string originId)
            => Task.FromResult<object>(null);
    }
}
