using Sia.Shared.Data;
using System.Collections.Generic;


namespace Sia.Domain.Playbook
{
    public class EventType : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<Action> Actions { get; set; }
            = new List<Action>();
    }
}
