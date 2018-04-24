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
    public class AddToMap : IStateTransform<Map>
    {
        public List<string> OrderedValues { get; set; }

        /// <returns>True if value added, false otherwise</returns>
        public bool Apply(ref Map currentState)
        {
            if(OrderedValues.Count < 1)
            {
                return false;
            }

            var subMapInScope = currentState;
            foreach (var key in OrderedValues.Take(OrderedValues.Count - 1))
            {
                if(!subMapInScope.Children.TryGetValue(key, out var nextSubmap))
                {
                    var newChild = new Map();
                    subMapInScope.Children.Add(key, newChild);
                    subMapInScope = newChild;
                }
                else
                {
                    subMapInScope = nextSubmap;
                }
            }

            var toAdd = OrderedValues[OrderedValues.Count - 1];

            return subMapInScope.Values.Contains(toAdd)
                || subMapInScope.Values.Add(toAdd);
        }
    }

    public class AddToMapRule : IStateTransformRule<Map>
    {
        public PartitionMetadata Metadata { get; set; }

        public IStateTransform<Map> GetTransform(Event ev)
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
