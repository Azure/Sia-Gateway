using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.ServiceRepositories.Operations
{
    public interface IGet<TRequest, TReturn>
    {
        Task<TReturn> GetAsync(TRequest request);
    }
}
