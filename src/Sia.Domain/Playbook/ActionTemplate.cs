using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Domain.Playbook
{
    public class ActionTemplate
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<ActionTemplateSource> Sources { get; set; }
            = new List<ActionTemplateSource>();
    }
}
