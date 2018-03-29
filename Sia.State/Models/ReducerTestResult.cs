using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Models
{
    public class ReducerTestResult
    {
        public ReducerTest Test { get; set; }
        public bool TestPassed { get; set; }
        public object ActualFinalState { get; set; }
    }
}
