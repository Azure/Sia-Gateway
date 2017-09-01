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
        public ICollection<Ticket> Tickets { get; set; }
            = new HashSet<Ticket>();
        public ICollection<Engagement> Engagements { get; set; }
            = new HashSet<Engagement>();
        public ICollection<Event> Events { get; set; }
            = new HashSet<Event>();
    }
}
