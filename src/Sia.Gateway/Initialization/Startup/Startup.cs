using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sia.Data.Incidents;
using Sia.Gateway.Initialization;
using Sia.Gateway.Initialization.Configuration;
using Sia.Core.Validation;
using System.Globalization;
using Sia.State.Services;

namespace Sia.Gateway
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            const string KUBERNETES_STACK = "K8SSTACK";
            const string STAGING_DB_CONNECTION_STRING_KEY = "incidentStaging";

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

            if (env.IsStaging())
            {
                _gatewayConfiguration.GatewayDatabaseConnectionString = _configuration.GetConnectionString(STAGING_DB_CONNECTION_STRING_KEY);
            }
            else if (env.IsEnvironment(KUBERNETES_STACK))
            {
                ThrowIf.NullOrWhiteSpace(_gatewayConfiguration.ClientSecret, nameof(_gatewayConfiguration.ClientSecret));
                ThrowIf.NullOrWhiteSpace(_gatewayConfiguration.KeyVaultAccessor.VaultName,
                    nameof(_gatewayConfiguration.KeyVaultAccessor.VaultName));
                ThrowIf.NullOrWhiteSpace(_gatewayConfiguration.KeyVaultAccessor.GatewayDatabaseConnectionStringName,
                    nameof(_gatewayConfiguration.KeyVaultAccessor.GatewayDatabaseConnectionStringName));
                ThrowIf.NullOrWhiteSpace(_gatewayConfiguration.KeyVaultAccessor.GatewayRedisPasswordName,
                    nameof(_gatewayConfiguration.KeyVaultAccessor.GatewayRedisPasswordName));

                var keyVaultUrl = String.Format(
                    CultureInfo.InvariantCulture,
                    "https://{0}.vault.azure.net/",
                    _gatewayConfiguration.KeyVaultAccessor.VaultName);
                builder.AddAzureKeyVault(keyVaultUrl, _gatewayConfiguration.ClientId, _gatewayConfiguration.ClientSecret);
                _configuration = builder.Build();

                var accessor = _gatewayConfiguration.KeyVaultAccessor;
                var connectionString = _configuration[accessor.GatewayDatabaseConnectionStringName];
                ThrowIf.NullOrWhiteSpace(connectionString, accessor.GatewayDatabaseConnectionStringName);
                _gatewayConfiguration.GatewayDatabaseConnectionString = connectionString;

                var redisPassword = _configuration[accessor.GatewayRedisPasswordName];
                ThrowIf.NullOrWhiteSpace(redisPassword, accessor.GatewayRedisPasswordName);
                _gatewayConfiguration.Redis.Password = redisPassword;
            }
            var appInsightsTask = _gatewayConfiguration.InitializeApplicationInsights();
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
            IncidentContext incidentContext,
            IReducerService reducerService)
        {
            LoggingStartup.AddLogging(env, loggerFactory, serviceProvider, _configuration);

            app.AddMiddleware(env, _gatewayConfiguration);

            AutoMapperStartup.InitializeAutomapper();

            if (_env.IsDevelopment())
            {
                SeedType seedType = _configuration["seedDataType"] == "manyEvents" ? SeedType.ManyEvents : SeedType.Basic;

                SeedData.Add(incidentContext, seedType);
            }

            // Warm up Reducer Service by loading, converting, and validating config
            reducerService.GetReducersAsync().Wait();
        }
    }
}
