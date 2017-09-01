using System.Threading.Tasks;

namespace Sia.Connectors.Tickets
{
    public abstract class Client<TTicket>
    {
        public abstract Task<TTicket> GetAsync(string originId);
    }
}
