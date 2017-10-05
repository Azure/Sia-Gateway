using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Data.Playbooks.Models
{
    public class ConditionSource : Source
    {
        public ICollection<Condition> Conditions { get; set; }
    }
}
