using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Data.Playbooks.Models
{
    public class EventType : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public dynamic Data { get; set; }
        public ICollection<EventTypeToActionAssociation> ActionAssociations { get; set; }
            = new HashSet<EventTypeToActionAssociation>();
        public ICollection<Action> Actions
            => new ManyToManyCollection<EventType, EventTypeToActionAssociation, Action>(this, ActionAssociations);

    }
}
