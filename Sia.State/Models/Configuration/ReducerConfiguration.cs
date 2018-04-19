using Sia.State.Models.Configuration.Validation;
using System.Collections.Generic;

namespace Sia.State.Models.Configuration
{
    public class ReducerConfiguration
    {
        public object InitialState { get; set; }
        public IList<ReducerCase> Cases { get; set; }
            = new List<ReducerCase>();
        public IList<ReducerTest> SelfTests { get; set; }
            = new List<ReducerTest>();
    }
}
