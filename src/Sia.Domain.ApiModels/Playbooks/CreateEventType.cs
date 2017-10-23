using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sia.Domain.ApiModels.Playbooks
{
    public class CreateEventType
    {
        [Required]
        public string Name { get; set; }
    }
}
