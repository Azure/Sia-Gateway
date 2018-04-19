using Microsoft.EntityFrameworkCore;
using Sia.Core.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Sia.Data.Incidents.Models
{
    // We should rename this, but that change should be coordinated
    // to include the Domain classes as well and be applicable across microservices
#pragma warning disable CA1716 // Identifiers should not match keywords
    public class Event : IEntity, IJsonDataString
#pragma warning restore CA1716 // Identifiers should not match keywords
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
