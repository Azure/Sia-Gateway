using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Data.Playbooks.Models
{
    public class EventTypeToActionAssociation
        : BidrectionalAssociation<EventType, Action>
    {
        public long EventTypeId { get; set; }
        public long ActionId { get; set; }
        public EventType EventType { get; set; }
        public Action Action { get; set; }

        protected override long _leftId { get => EventTypeId; set => EventTypeId = value; }
        protected override long _rightId { get => ActionId; set => ActionId = value; }
        protected override EventType _left { get => EventType; set => EventType = value; }
        protected override Action _right { get => Action; set => Action = value; }
    }
}
