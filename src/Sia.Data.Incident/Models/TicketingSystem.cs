using Sia.Shared.Data;

namespace Sia.Data.Incidents.Models
{
    public class TicketingSystem : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
