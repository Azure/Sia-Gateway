using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sia.Data.Incidents;
using Sia.Gateway.Initialization;
using Sia.Gateway.Initialization.Configuration;
using Sia.Shared.Authentication;
using System;

namespace Sia.Gateway
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            _configuration = builder.Build();
            _gatewayConfiguration = _configuration.Get<GatewayConfiguration>();
            var appInsightsTask = env.InitializeApplicationInsights(_gatewayConfiguration);
            appInsightsTask.Wait();
            _env = env;
        }

        private IHostingEnvironment _env { get; }
        private IConfigurationRoot _configuration { get; }

        private readonly GatewayConfiguration _gatewayConfiguration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddFirstPartyServices(_env, _configuration, _gatewayConfiguration);

            services.AddThirdPartyServices(_env, _gatewayConfiguration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider,
            IncidentContext incidentContext)
        {
            LoggingStartup.AddLogging(env, loggerFactory, serviceProvider, _configuration);

            app.AddMiddleware(env, _gatewayConfiguration);

            AutoMapperStartup.InitializeAutomapper();

            if (_env.IsDevelopment())
            {
                SeedType seedType = _configuration["seedDataType"] == "manyEvents" ? SeedType.ManyEvents : SeedType.Basic;

                SeedData.Add(incidentContext, seedType);
            }
        }
    }
}
