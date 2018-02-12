using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace Sia.Gateway.Initialization
{
    public static class LoggingStartup
    {
        public static void AddLogging(IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider provider, IConfigurationRoot configuration)
        {
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (!env.IsDevelopment()) loggerFactory.AddApplicationInsights(provider);
        }
    }
}
