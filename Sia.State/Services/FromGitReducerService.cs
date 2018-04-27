using Microsoft.Extensions.Logging;
using Sia.Core.Configuration.Sources.Git;
using Sia.Core.Validation;
using Sia.State.Configuration;
using Sia.State.Processing.Reducers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sia.State.Services
{
    public class FromGitReducerService
        : IReducerService
    {
        public FromGitReducerService(
            GitConfiguration config,
            ILoggerFactory loggerFactory
        )
        {
            GitConfig = ThrowIf.Null(config, nameof(config));
            Logger = ThrowIf.Null(loggerFactory, nameof(loggerFactory))
                .CreateLogger<FromGitHubReducerService>();
        }

        public GitConfiguration GitConfig { get; }
        private ILogger Logger { get; }
        private CombinedReducer Reducer { get; set; }

        public async Task<CombinedReducer> GetReducersAsync()
        {
            if (Reducer is null)
            {
                await BootstrapReducer()
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            return Reducer;
        }

        private async Task BootstrapReducer()
        {
            var reducerConfig = await GitConfig
                .GetRootReducerConfig(Logger)
                .ConfigureAwait(continueOnCapturedContext: false);

            var evaluatedReducers = reducerConfig.ResolveConfiguration();

            lock (Reducer)
            {
                if (Reducer is null)
                {
                    Reducer = evaluatedReducers;
                }
            }
        }
    }
}
