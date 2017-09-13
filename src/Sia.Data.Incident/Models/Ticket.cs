using Sia.Shared.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sia.Data.Incidents.Models
{
    public class Ticket : IEntity
    {
        public long Id { get; set; }
        public string OriginId { get; set; }
        [ForeignKey(nameof(TicketingSystem))]
        public long TicketingSystemId { get; set; }
        [ForeignKey(nameof(Incident))]
        public long IncidentId { get; set; }
        public bool IsPrimary { get; set; }

        public TicketingSystem TicketingSystem { get; set; }
        public Incident Incident { get; set; }
    }
}
