using Sia.Core.Configuration.Sources.Git;
using Sia.Core.Configuration.Sources.GitHub;
using Sia.State.Configuration;
using Sia.State.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddReducersFromCode<TReducerService>(
            this IServiceCollection services
        ) where TReducerService : class, IReducerService
            => services.AddSingleton<IReducerService, TReducerService>();

        public static IServiceCollection AddReducersFromGit(
            this IServiceCollection services,
            GitConfiguration config
        ) => services
                .AddSingleton(config)
                .AddSingleton<IReducerService, FromGitReducerService>();

        public static IServiceCollection AddReducersFromGithub(
            this IServiceCollection services,
            GitHubConfiguration config
        ) => services
                .AddSingleton(config)
                .AddSingleton<IReducerService, FromGitHubReducerService>();
    }
}
