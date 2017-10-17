﻿using Microsoft.EntityFrameworkCore;
using Sia.Shared.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sia.Data.Incidents.Models
{
    public class Event : IEntity, IDynamicDataStorage
    {
        public long Id { get; set; }
        public long? IncidentId { get; set; }
        public long EventTypeId { get; set; }
        public DateTime Occurred { get; set; }
        public DateTime EventFired { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string Data { get; set; }
    }
}
