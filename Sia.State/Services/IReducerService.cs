using Sia.State.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sia.State.Services
{
    public interface IReducerService
    {
        Task<CombinedReducers> GetReducersAsync();
    }
}
