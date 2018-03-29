using Sia.State.Models.MetadataTypes;
using Sia.State.Models.MetadataTypes.Transform;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Models
{
    public class StateTransform
    {
        public string DestinationFinalKey { get; set; }
        public string TransformType { get; set; }
        public object TransformMetadata { get; set; }


        public static Dictionary<string, Type> ValidTransformTypes() 
            => new Dictionary<string, Type>
            {
                { "Copy", typeof(SourceMetadata) },
                { "Increment", typeof(NoMetadata) },
                { "Decrement", typeof(NoMetadata) },
                { "AddToMap", typeof(PartitionMetadata) },
                { "RemoveFromMap", typeof(PartitionMetadata) }
            };
    }
}
