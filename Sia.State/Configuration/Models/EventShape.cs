using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Configuration.Models
{
    public class EventShape
    {
        public long EventTypeId { get; set; }
        public string RequiredDataKey { get; set; }
        public string RequiredDataValue { get; set; }
    }
}
