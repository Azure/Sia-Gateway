using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Data.Playbooks.Models
{
    public class Condition : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ConditionType ConditionType { get; set; }
        public string ComparisonValue { get; set; }
        public ConditionSource ConditionSource { get; set; }
        public long ConditionSourceId { get; set; }
        public ConditionSet ConditionSet { get; set; }
        public long ConditionSetId { get; set; }
    }

    public enum ConditionType
    {
        Equals,
        DoesNotEqual,
        Contains,
        DoesNotContain
    }
}
