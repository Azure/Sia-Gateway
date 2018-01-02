using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.Loader;

namespace Sia.Gateway.Initialization
{
    public static partial class Initialization
    {
        public static void LoadConnectorFromAssembly(
            this IServiceCollection services,
            IHostingEnvironment env,
            IConfigurationRoot config,
            string ticketConnectorAssemblyPath,
            string assemblyLoaderType = "Sia.Gateway.Initialization.LoadAssembly"
        )
        {
            var connectorAssembly = AssemblyLoadContext
                .Default
                .LoadFromAssemblyPath(ticketConnectorAssemblyPath);
            var connectorInitializerType = connectorAssembly
                .GetType(assemblyLoaderType);

            var connectorConfigureServices = connectorInitializerType
                .GetMethod(
                    "AddConnector",
                    new Type[] {
                        typeof(IServiceCollection),
                        typeof(IConfigurationRoot),
                        typeof(IHostingEnvironment)
                    }
                );
            connectorConfigureServices.Invoke(
                null, 
                new object[] { services, config, env }
            );
        }
    }
}
