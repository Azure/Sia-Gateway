using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Data.Playbooks.Models
{
    public class ConditionSet : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ConditionSetType Type { get; set; }
        public Action Action { get; set; }
        public long ActionId { get; set; }
        public ICollection<Condition> Conditions { get; set; }
            = new HashSet<Condition>();
    }

    public enum ConditionSetType
    {
        AnyOf,
        AllOf,
        NoneOf
    }
}
