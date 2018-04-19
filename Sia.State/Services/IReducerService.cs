using Sia.State.Models;
using Sia.State.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sia.State.Services
{
    public interface IReducerService
    {
        Task<CombinedReducerConfiguration> GetReducersAsync();
    }
}
