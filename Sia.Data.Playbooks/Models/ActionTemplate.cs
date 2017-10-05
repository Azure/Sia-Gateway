using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Data.Playbooks.Models
{
    public class ActionTemplate : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsUrl { get; set; }
        public string Template { get; set; }
        public ICollection<Action> Actions { get; set; }
            = new HashSet<Action>();
        public ICollection<ActionTemplateSource> Sources { get; set; }
            = new HashSet<ActionTemplateSource>();

    }
}
