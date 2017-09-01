using System;

namespace Sia.Data.Incidents.Models
{
    public class Engagement
    {
        public long Id { get; set; }
        public long IncidentId { get; set; }
        public DateTime TimeEngaged { get; set; }
        public DateTime? TimeDisengaged { get; set; }
        public Participant Participant { get; set; }
    }
}
