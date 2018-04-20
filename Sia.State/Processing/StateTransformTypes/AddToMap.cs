using Sia.State.Generation.Transform;
using Sia.State.MetadataTypes.Transform;
using Sia.State.Processing.StateSliceTypes;
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
                if(!subMapInScope.Children.TryGetValue(key, out subMapInScope))
                {
                    var newChild = new Map();
                    subMapInScope.Children.Add(key, newChild);
                    subMapInScope = newChild;
                }
            }

            var toAdd = OrderedValues[OrderedValues.Count - 1];

            return subMapInScope.Values.Contains(toAdd)
                || subMapInScope.Values.Add(toAdd);
        }
    }

    public class AddToMapRule : IStateTransformRule<PartitionMetadata, AddToMap>
    {
        public PartitionMetadata Metadata { get; set; }

        public AddToMap GetTransform(EventForAggregation ev) => throw new System.NotImplementedException();
    }
}
