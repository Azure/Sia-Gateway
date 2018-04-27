using Microsoft.Extensions.Logging;
using Sia.Core.Configuration.Sources.GitHub;
using Sia.State.Configuration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sia.State.Configuration
{
    public static class LoadReducerConfigurationFromGithub
    {
        private const string ApplicationName = "Sia-State";

        public static async Task<CombinedReducerConfiguration> GetRootReducerConfig(
            this GitHubConfiguration config,
            ILogger logger
        )
        {
            await config.EnsureValidTokenAsync().ConfigureAwait(continueOnCapturedContext: false);

            var client = config.Source.GetClient(ApplicationName);

            var reducersByDepth = (await client
                .GetSeedDataFromGitHub<ReducerConfiguration>(logger, config.Source.Repository, string.Empty)
                .ConfigureAwait(continueOnCapturedContext: false))
                .Select(pathToConfig => (pathTokens: pathToConfig.filePath.Split('/'), reducerConfig: pathToConfig.resultObject))
                .GroupBy(tokensToReducer => tokensToReducer.pathTokens.Count())
                .ToList();

            return reducersByDepth.ToCombinedReducerConfiguration();
        }

        public static CombinedReducerConfiguration ToCombinedReducerConfiguration(
            this IEnumerable<IGrouping<int, (string[] pathTokens, ReducerConfiguration reducerConfig)>> layers,
            int targetLayer = 1
        ) => new CombinedReducerConfiguration()
        {
            CompositeChildren = layers
                .Where(group => group.Key > targetLayer)
                .SelectMany(a => a)
                .GroupBy(tokensToReducer => tokensToReducer.pathTokens[targetLayer - 1])
                .Select(groupedConfigs => new KeyValuePair<string, CombinedReducerConfiguration>(
                    groupedConfigs.Key,
                    groupedConfigs
                        .GroupBy(groupedConfig => groupedConfig.pathTokens.Count())
                        .ToList()
                        .ToCombinedReducerConfiguration(targetLayer + 1)))
                .ToDictionary(),
            SimpleChildren = layers
                .First(group => group.Key == targetLayer)
                .Select(tokensToReducer => new KeyValuePair<string, ReducerConfiguration>(
                    tokensToReducer.pathTokens[targetLayer - 1],
                    tokensToReducer.reducerConfig))
                .ToDictionary()
        };
    }
}
