using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Data.Playbooks.Models
{
    public class Conditions
    {
        public long Id { get; set; }
    }

    public enum ConditionSetType
    {
        AnyOf,
        AllOf,
        NoneOf
    }
}
