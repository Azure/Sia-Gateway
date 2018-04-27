using Newtonsoft.Json.Linq;
using Sia.Data.Incidents.Models;
using Sia.State.MetadataTypes.Transform;
using Sia.State.Processing.StateModels;
using Sia.State.Processing.Transforms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sia.State.Processing.Transforms
{
    public class AddToMap : IStateTransform<Tree>
    {
        public List<string> OrderedValues { get; set; }

        /// <returns>True if value added, false otherwise</returns>
        public bool Apply(ref Tree currentState)
        {
            if(OrderedValues.Count < 1)
            {
                return false;
            }

            var subMapInScope = currentState;
            foreach (var key in OrderedValues.Take(OrderedValues.Count - 1))
            {
                if(!subMapInScope.Branches.TryGetValue(key, out var nextSubmap))
                {
                    var newChild = new Tree();
                    subMapInScope.Branches.Add(key, newChild);
                    subMapInScope = newChild;
                }
                else
                {
                    subMapInScope = nextSubmap;
                }
            }

            var toAdd = OrderedValues[OrderedValues.Count - 1];

            return subMapInScope.Leaves.Contains(toAdd)
                || subMapInScope.Leaves.Add(toAdd);
        }
    }

    public class AddToMapRule 
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

            return new AddToMap()
            {
                OrderedValues = orderedValues
            };
        }
    }
}
