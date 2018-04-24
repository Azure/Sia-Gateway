using Sia.State;
using Sia.State.Configuration;
using Sia.State.Processing.Reducers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sia.State.Services
{
    public interface IReducerService
    {
        Task<CombinedReducer> GetReducersAsync();
    }
}
