using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Domain.Playbook
{
    public class Condition
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ConditionType ConditionType { get; set; }
        public string ComparisonValue { get; set; }
        public ConditionSource ConditionSource { get; set; }
    }

    public enum ConditionType
    {
        Equals,
        NotEquals,
        Contains,
        DoesNotContain
    }
}
