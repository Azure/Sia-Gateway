using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.ServiceRepositories.Operations
{
    public interface IGetMany<TRequest, TReturn>
    {
        Task<IEnumerable<TReturn>> GetManyAsync(TRequest request);
    }
}
