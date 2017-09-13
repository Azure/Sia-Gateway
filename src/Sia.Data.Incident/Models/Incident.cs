using Sia.Shared.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sia.Data.Incidents.Models
{
    public class Incident : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
            = new HashSet<Ticket>();
        public ICollection<Engagement> Engagements { get; set; }
            = new HashSet<Engagement>();
        public ICollection<Event> Events { get; set; }
            = new HashSet<Event>();
    }
}
