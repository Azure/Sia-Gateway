using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sia.Data.Incidents.Models;
using Sia.State.Generation.Transform;
using Sia.State.MetadataTypes.Transform;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Processing.StateTransformTypes
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
