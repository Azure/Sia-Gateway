using System.Collections.Generic;

namespace Sia.State.Models
{
    public class Reducer
    {
        public object InitialState { get; set; }
        public IList<ReducerCase> Cases { get; set; }
            = new List<ReducerCase>();
        public IList<ReducerTest> SelfTests { get; set; }
            = new List<ReducerTest>();
    }
}
