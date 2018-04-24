using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sia.Core.Validation;
using Sia.State;
using Sia.State.Processing.Reducers;

namespace Sia.State.Services
{
    public class FromCodeReducerService
        : IReducerService
    {
        private readonly CombinedReducer _reducers;

        public FromCodeReducerService(CombinedReducer reducers)
        {
            _reducers = ThrowIf.Null(reducers, nameof(reducers));
        }
        public Task<CombinedReducer> GetReducersAsync()
            => Task.FromResult(_reducers);
    }
}
