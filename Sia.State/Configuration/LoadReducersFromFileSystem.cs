using Microsoft.Extensions.Logging;
using Sia.Core.Configuration.Sources.FileSystem;
using Sia.Core.Configuration.Sources.GitHub;
using Sia.State.Configuration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sia.State.Configuration
{
    public static class LoadReducersFromFileSystem
    {
        public static Task<CombinedReducerConfiguration> GetRootReducerConfig(
            this IPathConfig config,
            ILogger logger
        ) => Task.FromResult(
            config
                .GetDataFromLocal<ReducerConfiguration>(logger)
                .Select(fileInfoToReducer => (
                    pathTokens: fileInfoToReducer.file.ToPathTokens(config.Path),
                    reducerConfig: fileInfoToReducer.record
                ))
                .GroupBy(tokensToReducer => tokensToReducer.pathTokens.Count())
                .ToCombinedReducerConfiguration()
        );

    }
}
