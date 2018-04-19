using Sia.State.Models.MetadataTypes.Transform;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Models.Processing
{
    public class StateTransformDefinition
    {
        public StateTransformDefinition(StateTransformConfiguration config)
        {
            TransformData = (ITransformMetadata)config.TransformMetadata;
        }

        public ITransformMetadata TransformData { get; set; }
    }

    public static class StateTransformDefinitionFactory
    {
        public static ToDefinition(this StateTransformConfiguration config)
    }
}
