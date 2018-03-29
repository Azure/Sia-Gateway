using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Models
{
    public class CombinedReducers
    {
        public Dictionary<string, CombinedReducers> ComplexChildren { get; set; }
            = new Dictionary<string, CombinedReducers>();
        public Dictionary<string, Reducer> SimpleChildren { get; set; }
            = new Dictionary<string, Reducer>();
    }
}
