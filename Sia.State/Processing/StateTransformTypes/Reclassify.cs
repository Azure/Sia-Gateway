using Sia.State.Generation.Transform;
using Sia.State.Processing.StateSliceTypes;
using System.Collections.Generic;

namespace Sia.State.Processing.StateTransformTypes
{
    public class Reclassify : IStateTransform<Map>
    {
        public List<string> SourceOrderedValues { get; set; }
        public List<string> DestinationOrderedValues { get; set; }

        /// <returns>True if value existed in source AND was removed from source AND was added to destination.</returns>
        public bool Apply(ref Map currentState)
        {
            var remove = new RemoveFromMap()
            {
                OrderedValues = SourceOrderedValues
            };
            var add = new AddToMap()
            {
                OrderedValues = DestinationOrderedValues
            };

            return remove.Apply(ref currentState)
                && add.Apply(ref currentState);
        }
    }
}
