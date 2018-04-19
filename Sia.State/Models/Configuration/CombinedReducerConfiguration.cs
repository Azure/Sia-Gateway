using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Models.Configuration
{
    public class CombinedReducerConfiguration
    {
        public Dictionary<string, CombinedReducerConfiguration> ComplexChildren { get; set; }
            = new Dictionary<string, CombinedReducerConfiguration>();
        public Dictionary<string, ReducerConfiguration> SimpleChildren { get; set; }
            = new Dictionary<string, ReducerConfiguration>();
    }
}
