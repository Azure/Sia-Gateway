using Newtonsoft.Json.Linq;
using Sia.Data.Incidents.Models;
using Sia.State.MetadataTypes.Transform;

namespace Sia.State.Processing.Transforms
{
    public class CopyFromSource
        : IStateTransform<string>
    {
        public string NewValue { get; set; }

        public bool Apply(ref string currentState)
        {
            currentState = NewValue;
            return true;
        }
    }

    public class CopyFromSourceRule : StateTransformRule<PathMetadata, string>
    {
        public override IStateTransform<string> GetTransform(Event ev)
            => new CopyFromSource()
            {
                NewValue = JObject.Parse(ev.Data).SelectToken(Metadata.Key).ToString()
            };
    }
}
