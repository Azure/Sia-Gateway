using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Data.Playbooks.Models
{
    public class Condition
    {
        public long Id { get; set; }
        public ConditionType ConditionType { get; set; }
        public string Value { get; set; }
        public Variable Variable { get; set; }
    }

    public enum ConditionType
    {
        Equals,
        NotEquals,
        Contains,
        DoesNotContain
    }
}
