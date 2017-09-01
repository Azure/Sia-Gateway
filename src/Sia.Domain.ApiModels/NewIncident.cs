using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sia.Domain.ApiModels
{
    public class NewIncident
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public Ticket PrimaryTicket { get; set; }
        public IList<Ticket> Tickets { get; set; }
            = new List<Ticket>();
        public IList<NewEvent> Events { get; set; }
            = new List<NewEvent>();
        public IList<Participant> Engagements { get; set; }
            = new List<Participant>();
    }
}
