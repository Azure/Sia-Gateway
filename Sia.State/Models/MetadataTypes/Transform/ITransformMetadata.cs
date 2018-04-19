using Sia.State.Generation.Transform;
using Sia.State.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Models.MetadataTypes.Transform
{
    public interface ITransformMetadata
    {
        StateTransform GetTransform(EventForAggregation ev);
    }
}
