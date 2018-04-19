using Sia.State.Generation.Transform;
using Sia.State.Models.MetadataTypes.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.State.Models.Processing.StateSliceTypes
{
    public class Map
    {
        public Dictionary<string, Map> Children { get; private set; }
            = new Dictionary<string, Map>(StringComparer.InvariantCultureIgnoreCase);
        public HashSet<string> Values { get; private set; }
            = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
    }

    public class AddToMap : IStateTransform<Map>
    {
        public List<string> OrderedValues { get; set; }

        /// <returns>True if value added, false otherwise</returns>
        public bool Apply(Map currentState)
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

    public class RemoveFromMap : IStateTransform<Map>
    {
        public List<string> OrderedValues { get; set; }

        /// <returns>True if object was removed, false if no change</returns>
        public bool Apply(Map currentState)
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

    public class Reclassify : IStateTransform<Map>
    {
        public List<string> SourceOrderedValues { get; set; }
        public List<string> DestinationOrderedValues { get; set; }

        /// <returns>True if value existed in source AND was removed from source AND was added to destination.</returns>
        public bool Apply(Map currentState)
        {
            var remove = new RemoveFromMap()
            {
                OrderedValues = SourceOrderedValues
            };
            var add = new AddToMap()
            {
                OrderedValues = DestinationOrderedValues
            };

            return remove.Apply(currentState)
                && add.Apply(currentState);
        }
    }
}
