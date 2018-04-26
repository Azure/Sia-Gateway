using Sia.Core.Validation;
using Sia.State;
using Sia.State.Configuration.Models;
using Sia.State.Configuration.Models.Validation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sia.State.Services
{
    public class ReducerTestingService
    {
        private readonly IReducerService _reducerService;

        public ReducerTestingService(
            IReducerService reducerService    
        )
        {
            _reducerService = ThrowIf.Null(reducerService, nameof(reducerService));
        }

        public async Task<List<ReducerTestResult>> RunTestsAsync()
        {
            var reducers = await _reducerService.GetReducersAsync();


            // TODO: Implement
            return null;
        }

        private Action<CombinedReducerConfiguration, string> TestRecursively => (CombinedReducerConfiguration reducer, string testNamespace) =>
        {
            var complexChildTests = reducer.CompositeChildren
                .ToAsyncEnumerable()
                .Select(kvp => (child: kvp.Value, testNamespace: testNamespace + kvp.Key))
                .ForEachAsync(result => TestRecursively(result.child, result.testNamespace));

            /*var simpleChildTests = reducer.SimpleChildren
                .ToAsyncEnumerable()
                .Select(kvp => (child: kvp.Value, testNamespace: testNamespace + kvp.Key))
                .ForEachAsync(result =>
                    result.child.SelfTests
                       .ToAsyncEnumerable()
                       .ForEachAsync(
                );*/

            // TODO: Implement
        };
    }
}
