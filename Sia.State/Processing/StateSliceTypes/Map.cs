using Sia.State.MetadataTypes.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.State.Processing.StateSliceTypes
{
    public class Map
    {
        public Dictionary<string, Map> Children { get; private set; }
            = new Dictionary<string, Map>(StringComparer.InvariantCultureIgnoreCase);
        public HashSet<string> Values { get; private set; }
            = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
    }
}
