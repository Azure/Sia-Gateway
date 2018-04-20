using Sia.State.Configuration.Validation;
using Sia.State.Processing.StateSliceTypes;
using System;
using System.Collections.Generic;

namespace Sia.State.Configuration
{
    public class ReducerConfiguration
    {
        public object InitialState { get; set; }
        public string StateType { get; set; }
        public IList<ReducerCaseConfiguration> Cases { get; set; }
            = new List<ReducerCaseConfiguration>();
        public IList<ReducerTestConfiguration> SelfTests { get; set; }
            = new List<ReducerTestConfiguration>();

        public static Dictionary<string, Type> ValidStateTypes
            = new Dictionary<string, Type>()
            {
                { nameof(Map), typeof(Map) }
            };
    }
}
