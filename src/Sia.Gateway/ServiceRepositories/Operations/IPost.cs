using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.ServiceRepositories.Operations
{
    public interface IPost<TRequest, TReturn>
    {
        Task<TReturn> PostAsync(TRequest request);
    }
}
