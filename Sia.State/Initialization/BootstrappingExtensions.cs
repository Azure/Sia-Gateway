using Sia.State.Configuration;
using Sia.State.Processing.Reducers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.State.Initialization
{
    public static class BootstrappingExtensions
    {
        public static CombinedReducer ResolveConfiguration(this CombinedReducerConfiguration config, string reducerName = "Base")
        {
            var toReturn = new CombinedReducer();
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

            //TODO: FINISH
            return toReturn;
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
                .Select(rCase => rCase.ResolveConfiguration<TState>())
                .ToList();

            var reducer = new Reducer<TState>()
            {
                InitialState = (TState)config.InitialState,
                ApplicableEvents = reducerCases
                    .Select(rCase => rCase.MatchTriggeringEvents)
                    .ToUnionFilter(),
                Cases = reducerCases
            };

            return reducer;
        }

        public static ReducerCase<TState> ResolveConfiguration<TState>(this ReducerCaseConfiguration config)
        {
            var rCase = new ReducerCase<TState>();

            //TODO: FINISH
            return rCase;
        }
    }
}
