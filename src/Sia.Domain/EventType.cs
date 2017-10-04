using Sia.Shared.Data;

namespace Sia.Domain
{
    public class EventType : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
