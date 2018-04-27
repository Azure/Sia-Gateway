using Sia.State.Configuration.Models.Validation;
using Sia.State.Processing.StateModels;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static Dictionary<string, Type> ValidStateTypes { get; }
            = new Dictionary<string, Type>()
            {
                { nameof(Tree), typeof(Tree) }
            };
    }

    public static class ReducerConfigurationHelperExtensions
    {
        public static Dictionary<string, Type> ThrowIfAnyInvalidTypes(this Dictionary<string, Type> stateTypes)
        {
            stateTypes
                .Where(nameTypeKvp =>
                    !nameTypeKvp.Value
                    .GetInterfaces()
                    .Contains(typeof(IDeepCopyable<>).MakeGenericType(nameTypeKvp.Value)))
                .Select<KeyValuePair<string, Type>, object>(nameTypeKvp => throw new InvalidStateTypeConfiguredException(nameTypeKvp.Key, "IDeepCopyable"));
            return stateTypes;
        }
    }

    public class InvalidStateTypeConfiguredException : Exception
    {
        public InvalidStateTypeConfiguredException(string name, string interfaceMissing)
            : base($"Type named {name} does not implement {interfaceMissing} and is thus not a valid state type. The end user has no control over this, please consult the devloper")
        { }
    }
}
