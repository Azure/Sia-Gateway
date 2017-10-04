using Sia.Shared.Data;
using System;

namespace Sia.Data.Incidents.Models
{
    public class Engagement : IEntity
    {
        public long Id { get; set; }
        public long IncidentId { get; set; }
        public DateTime TimeEngaged { get; set; }
        public DateTime? TimeDisengaged { get; set; }
        public Participant Participant { get; set; }
    }
}
