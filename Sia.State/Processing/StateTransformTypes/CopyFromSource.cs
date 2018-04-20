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

    public class CopyFromSourceRule : IStateTransformRule<PathMetadata, CopyFromSource>
    {
        public PathMetadata Metadata { get; set; }

        public CopyFromSource GetTransform(EventForAggregation ev)
            => new CopyFromSource() { NewValue = ev.Data.SelectToken(Metadata.Key).ToString() };
    }
}
