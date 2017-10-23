using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Domain.Playbook
{
    public abstract class Source
    {
        public long Id { get; set; }
        public SourceObject SourceObject { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }

    public enum SourceObject
    {
        Event,
        Ticket,
        EventType,
        Engagement
    }
}
