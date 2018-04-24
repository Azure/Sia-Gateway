using Sia.State.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddReducersFromCode<TReducerService>(this IServiceCollection services)
            where TReducerService : class, IReducerService
            => services.AddSingleton<IReducerService, TReducerService>(); 
    }
}
