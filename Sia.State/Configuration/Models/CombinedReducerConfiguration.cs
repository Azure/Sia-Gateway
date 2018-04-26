using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Configuration.Models
{
    public class CombinedReducerConfiguration
    {
        public IDictionary<string, CombinedReducerConfiguration> CompositeChildren { get; set; }
            = new Dictionary<string, CombinedReducerConfiguration>(StringComparer.InvariantCultureIgnoreCase);
        public IDictionary<string, ReducerConfiguration> SimpleChildren { get; set; }
            = new Dictionary<string, ReducerConfiguration>(StringComparer.InvariantCultureIgnoreCase);
    }
}
