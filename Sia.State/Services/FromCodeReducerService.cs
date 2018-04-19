using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sia.Core.Validation;
using Sia.State.Models;

namespace Sia.State.Services
{
    public class FromCodeReducerService
        : IReducerService
    {
        private readonly CombinedReducers _reducers;

        public FromCodeReducerService(CombinedReducers reducers)
        {
            _reducers = ThrowIf.Null(reducers, nameof(reducers));
        }
        public Task<CombinedReducers> GetReducersAsync()
            => Task.FromResult(_reducers);
    }
}
