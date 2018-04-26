using Sia.State.Configuration.Models.Validation;
using Sia.State.Processing.StateModels;
using System;
using System.Collections.Generic;

namespace Sia.State.Configuration.Models
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
                { nameof(Tree), typeof(Tree) }
            };
    }
}
