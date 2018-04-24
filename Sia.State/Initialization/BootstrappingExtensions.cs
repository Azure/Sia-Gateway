using Sia.State.Configuration;
using Sia.State.Processing.Reducers;
using Sia.State.Processing.StateTransformTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.State.Initialization
{
    public static class BootstrappingExtensions
    {
        public static CombinedReducer ResolveConfiguration(this CombinedReducerConfiguration config, string reducerName = "Root")
        {
            var compositeChildren = config.CompositeChildren
                .Select(kvp => new KeyValuePair<string, IReducer>(
                    kvp.Key,
                    kvp.Value.ResolveConfiguration($"{reducerName}.{kvp.Key}"))
                );
            var simpleChildren = config.SimpleChildren
                .Select(kvp => new KeyValuePair<string, IReducer>(
                    kvp.Key,
                    kvp.Value.ResolveConfiguration($"{reducerName}.{kvp.Key}"))
                );

            var children = compositeChildren.ToDictionary();
            foreach (var simpleChild in simpleChildren)
            {
                // If a folder and file with the same name exist,
                // The folder should be used.
                // TODO: Emit a warning when that happens
                if(!children.ContainsKey(simpleChild.Key))
                {
                    children.Add(simpleChild);
                }
            }

            var initialState = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var child in children)
            {
                initialState.Add(child.Key, child.Value.GetRawInitialState());
            }

            return new CombinedReducer()
            {
                Name = reducerName,
                InitialState = initialState,
                ApplicableEvents = children
                    .Select(kvp => kvp.Value.ApplicableEvents)
                    .ToUnionFilter(),
                Children = children
            };
        }

        public static IReducer ResolveConfiguration(this ReducerConfiguration config, string reducerName)
        {
            if (!ReducerConfiguration.ValidStateTypes.TryGetValue(config.StateType, out Type stateType))
            {
                throw new InvalidStateTypeException(config.StateType, reducerName);
            }

            var stronglyTypedResolveConfiguration = typeof(BootstrappingExtensions)
                .GetMethod(nameof(TypedResolveConfiguration), new Type[] { typeof(ReducerConfiguration), typeof(string) })
                .MakeGenericMethod(stateType);

            return (IReducer)stronglyTypedResolveConfiguration.Invoke(null, new object[] { config, reducerName });
        }

        public static Reducer<TState> TypedResolveConfiguration<TState>(this ReducerConfiguration config, string reducerName)
            where TState : class
        {
            var reducerCases = config.Cases
                .Select(rCase => rCase.ResolveConfiguration<TState>(reducerName))
                .ToList();

            var reducer = new Reducer<TState>()
            {
                Name = reducerName,
                InitialState = (TState)config.InitialState,
                ApplicableEvents = reducerCases
                    .Select(rCase => rCase.MatchTriggeringEvents)
                    .ToUnionFilter(),
                Cases = reducerCases
            };

            return reducer;
        }

        public static ReducerCase<TState> ResolveConfiguration<TState>(this ReducerCaseConfiguration config, string reducerName)
        {
            if (!StateTransformConfiguration.ValidTransformTypes.TryGetValue(
                config.StateTransformToApply.TransformType, 
                out (Type MetaDataType, Type TransformRuleType) transform))
            {
                throw new InvalidStateTypeException(config.StateTransformToApply.TransformType, reducerName);
            }

            // TODO: Validate Generic Type Restrictions for ResolveConfiguration
            var stronglyTypedResolveConfiguration = typeof(BootstrappingExtensions)
                .GetMethod(nameof(TypedResolveConfiguration), new Type[] { typeof(ReducerConfiguration), typeof(string) })
                .MakeGenericMethod(typeof(TState), transform.MetaDataType, transform.TransformRuleType);

            return (ReducerCase<TState>)stronglyTypedResolveConfiguration.Invoke(null, new object[] { config });
        }

        public static ReducerCase<TState> ResolveConfiguration<TState, TMetadata, TRule>(this ReducerCaseConfiguration config)
            where TRule : IStateTransformRule<TState>, ITransformMetadata<TMetadata>, new()
        {
            var rCase = new ReducerCase<TState>()
            {
                MatchTriggeringEvents = config.TriggeringEventShape.ToFilter(),
                StateTransformToApply = new TRule
                {
                    Metadata = (TMetadata)config.StateTransformToApply.TransformData
                }
            };

            return rCase;
        }
    }
}
