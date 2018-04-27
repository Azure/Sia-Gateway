using Newtonsoft.Json.Linq;
using Sia.Data.Incidents.Models;
using Sia.State.MetadataTypes.Transform;
using Sia.State.Processing.StateModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sia.State.Processing.Transforms
{
    public class RemoveFromMap : IStateTransform<Tree>
    {
        public List<string> OrderedValues { get; set; }

        /// <returns>True if object was removed, false if no change</returns>
        public bool Apply(ref Tree currentState)
        {
            if (OrderedValues.Count < 1)
            {
                return false;
            }

            var subMapInScope = currentState;
            foreach (var key in OrderedValues.Take(OrderedValues.Count - 1))
            {
                if (!subMapInScope.Branches.TryGetValue(key, out subMapInScope))
                {
                    return false; // No values in a map that doesn't exist
                }
            }

            var valueIsRemoved = subMapInScope.Leaves.Remove(OrderedValues[OrderedValues.Count - 1]);

            currentState.RemoveBranchesWithNoLeaves();

            return valueIsRemoved;
        }
    }

    public class RemoveFromMapRule 
        : StateTransformRule<PartitionMetadata, Tree>
    {
        public override IStateTransform<Tree> GetTransform(Event ev)
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
