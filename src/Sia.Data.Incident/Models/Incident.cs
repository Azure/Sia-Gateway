using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sia.Data.Incidents.Models
{
    public class Incident
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }
        [ForeignKey(nameof(PrimaryTicket))]
        public long PrimaryTicketId { get; set; }

        public Ticket PrimaryTicket { get; set; }
        [InverseProperty(nameof(Ticket.Incident))]
        public List<Ticket> Tickets { get; set; }
        public List<Engagement> Engagements { get; set; }
        public List<Event> Events { get; set; }
    }
}
