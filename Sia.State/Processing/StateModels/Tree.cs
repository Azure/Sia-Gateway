using Sia.State.MetadataTypes.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.State.Processing.StateModels
{
    public class Tree : IDeepCopyable<Tree>
    {
        public IDictionary<string, Tree> Branches { get; private set; }
            = new Dictionary<string, Tree>(StringComparer.InvariantCultureIgnoreCase);
        public HashSet<string> Leaves { get; private set; }
            = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        public Tree GetDeepCopy() => new Tree()
        {
            Branches = Branches
                .Select(childKvp => new KeyValuePair<string, Tree>(string.Copy(childKvp.Key), childKvp.Value.GetDeepCopy()))
                .ToDictionary(),
            Leaves = Leaves
                .Select(value => string.Copy(value))
                .ToHashSet()
        };

        public void RemoveBranchesWithNoLeaves()
        {
            var toRemove = new List<string>();
            foreach (var child in Branches)
            {
                child.Value.RemoveBranchesWithNoLeaves();
                if (child.Value.Leaves.Count == 0
                    && child.Value.Branches.Count == 0)
                {
                    toRemove.Add(child.Key);
                }
            }
            foreach (var bareBranch in toRemove)
            {
                Branches.Remove(bareBranch);
            }
        }
    }
}
