using Sia.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Models.Configuration.Validation
{
    public class ReducerTest
    {
        public string TestName { get; set; }
        public object InitialState { get; set; }
        public object ExpectedFinalState { get; set; }
        public IList<Event> InputEvents { get; set; }
            = new List<Event>();
    }
}
