using Microsoft.Extensions.Logging;
using Sia.Core.Configuration.Sources.Git;
using Sia.State.Configuration.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sia.State.Configuration
{
    public static class LoadReducersFromGit
    {
        public static Task<CombinedReducerConfiguration> GetRootReducerConfig(
            this GitConfiguration config,
            ILogger logger
        ) => Task.FromResult(config
                .GetDataFromGit<ReducerConfiguration>(logger)
                .Select(fileInfoToReducer => (
                    pathTokens: fileInfoToReducer.file.ToPathTokens(config.Source.CloneToPath),
                    reducerConfig: fileInfoToReducer.record
                ))
                .GroupBy(tokensToReducer => tokensToReducer.pathTokens.Count())
                .ToCombinedReducerConfiguration()
            );

        public static string[] ToPathTokens(
            this FileInfo file,
            string relativeToDirectory,
            string extension = ".json"
        ) => file.DirectoryName
            .Remove(0, relativeToDirectory.Length)
            .Split('/')
            .AsEnumerable()
            .Append(file.Name.Remove(file.Name.IndexOf(extension, StringComparison.InvariantCultureIgnoreCase), extension.Length))
            .ToArray();
    }
}
