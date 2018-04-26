using Sia.State.Processing.StateModels;
using System.Collections.Generic;

namespace Sia.State.Processing.Transforms
{
    public class Reclassify : IStateTransform<Tree>
    {
        public List<string> SourceOrderedValues { get; set; }
        public List<string> DestinationOrderedValues { get; set; }

        /// <returns>True if value existed in source AND was removed from source AND was added to destination.</returns>
        public bool Apply(ref Tree currentState)
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
