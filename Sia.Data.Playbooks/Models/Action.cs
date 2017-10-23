using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Data.Playbooks.Models
{
    public class Action : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ActionTemplate ActionTemplate { get; set; }
        public long ActionTemplateId { get; set; }
        public ICollection<ConditionSet> ConditionSets { get; set; }
            = new HashSet<ConditionSet>();
        public ICollection<EventTypeToActionAssociation> EventTypeAssociations { get; set; }
            = new HashSet<EventTypeToActionAssociation>();
        public ICollection<EventType> EventTypes
            => new ManyToManyCollection<Action, EventTypeToActionAssociation, EventType>(this, EventTypeAssociations);
    }
}
