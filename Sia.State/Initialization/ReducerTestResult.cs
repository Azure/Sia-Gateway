using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Configuration.Validation
{
    public class ReducerTestResult
    {
        public ReducerTestConfiguration Test { get; set; }
        public bool TestPassed { get; set; }
        public object ActualFinalState { get; set; }
    }
}
