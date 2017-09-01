using System;
using System.ComponentModel.DataAnnotations;

namespace Sia.Domain.ApiModels
{
    public class NewEvent
    {
        [Required]
        public long? EventTypeId { get; set; }
        [Required]
        public DateTime? Occurred { get; set; }
        [Required]
        public DateTime? EventFired { get; set; }
    }
}
