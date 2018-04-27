﻿using Sia.State.MetadataTypes;
using Sia.State.MetadataTypes.Transform;
using Sia.State.Processing.Transforms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State
{
    public class StateTransformConfiguration
    {
        public string TransformType { get; set; }
        public object TransformData { get; set; }

        // TODO validate types implement correct generics. See ReducerConfiguration.cs
        public static Dictionary<string, (Type MetaDataType, Type TransformRuleType)> ValidTransformTypes { get; }
            = new Dictionary<string, (Type MetaDataType, Type TransformRuleType)>
            {
                { "Copy", (typeof(PathMetadata),  typeof(CopyFromSourceRule))},
                { "AddToMap", (typeof(PartitionMetadata), typeof(AddToMapRule))},
                { "RemoveFromMap", (typeof(PartitionMetadata), typeof(RemoveFromMapRule))}
            };
    }
}
