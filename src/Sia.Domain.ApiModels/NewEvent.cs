using Sia.Shared.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace Sia.Domain.ApiModels
{
    public class NewEvent
        :IDynamicDataSource
    {
        [Required]
        public long? EventTypeId { get; set; }
        [Required]
        public DateTime? Occurred { get; set; }
        [Required]
        public DateTime? EventFired { get; set; }
        public dynamic Data { get; set; } = new ExpandoObject();
    }
}
