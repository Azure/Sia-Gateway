using System.Threading.Tasks;

namespace Sia.Connectors.Tickets.None
{
    public class NoClient : Client<EmptyTicket>
    {
        public override Task<EmptyTicket> GetAsync(string originId)
        {
            return Task.FromResult(new EmptyTicket());
        }
    }
}
