using Newtonsoft.Json.Linq;
using Sia.Data.Incidents.Models;
using Sia.State.Generation.Transform;
using Sia.State.MetadataTypes.Transform;
using Sia.State.Processing.StateSliceTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sia.State.Processing.StateTransformTypes
{
    public class RemoveFromMap : IStateTransform<Map>
    {
        public List<string> OrderedValues { get; set; }

        /// <returns>True if object was removed, false if no change</returns>
        public bool Apply(ref Map currentState)
        {
            if (OrderedValues.Count < 1)
            {
                return false;
            }

            var subMapInScope = currentState;
            foreach (var key in OrderedValues.Take(OrderedValues.Count - 1))
            {
                if (!subMapInScope.Children.TryGetValue(key, out subMapInScope))
                {
                    return false; // No values in a map that doesn't exist
                }
            }

            return subMapInScope.Values.Remove(OrderedValues[OrderedValues.Count - 1]);
        }
    }

    public class RemoveFromMapRule : StateTransformRule<PartitionMetadata, Map>
    {
        public override IStateTransform<Map> GetTransform(Event ev)
        {
            var jData = JObject.Parse(ev.Data);
            var orderedValues = Metadata.PartitionBySourceKeys
                .Select(key => jData
                    .GetValue(key, StringComparison.InvariantCultureIgnoreCase)
                    .ToObject<string>() ?? "Other")
                .ToList();

            return new RemoveFromMap()
            {
                OrderedValues = orderedValues
            };
        }
    }
}
