using System.Threading.Tasks;

namespace Sia.Connectors.Tickets
{
    public abstract class TicketingClient
    {
        public abstract Task<object> GetAsync(string originId);
    }
}
