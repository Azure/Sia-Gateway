using Microsoft.EntityFrameworkCore;
using Sia.Shared.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Sia.Data.Incidents.Models
{
    public class Event : IEntity, IJsonDataString
    {
        public long Id { get; set; }
        public long? IncidentId { get; set; }
        public Incident Incident { get; set; }
        public long EventTypeId { get; set; }
        public DateTime Occurred { get; set; }
        public DateTime EventFired { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string Data { get; set; }
    }
}
