using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.ServiceRepositories.Operations
{
    public interface IPut<TRequest>
    {
        Task PutAsync(TRequest request);
    }
}
