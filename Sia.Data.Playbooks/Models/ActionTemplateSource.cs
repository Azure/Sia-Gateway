using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Data.Playbooks.Models
{
    public class ActionTemplateSource : Source
    {
        public ActionTemplate ActionTemplate { get; set; }
        public long ActionTemplateId { get; set; }
        public string Name { get; set; }
    }
}
