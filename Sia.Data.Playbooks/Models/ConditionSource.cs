using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Data.Playbooks.Models
{
    public class ConditionSource : Source
    {
        public Condition Condition { get; set; }
        public long ConditionId { get; set; }
    }
}
