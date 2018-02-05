using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sia.Connectors.Tickets.None;
using Sia.Connectors.Tickets.TicketProxy;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Gateway.Requests;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Protocol;
using Sia.Shared.Validation;
using System;
using System.Buffers;
using System.Reflection;
using System.Runtime.Loader;

namespace Sia.Gateway.Initialization
{
    public static class ServicesStartup
    {

        public static void AddFirstPartyServices(
            this IServiceCollection services,
            IHostingEnvironment env,
            IConfigurationRoot config)
        {
            ConfigureAuth(services, config);


            if (env.IsDevelopment()) services.AddDbContext<IncidentContext>(options => options.UseInMemoryDatabase("Live"));
            if (env.IsStaging()) services.AddDbContext<IncidentContext>(options => options.UseSqlServer(config.GetConnectionString("incidentStaging")));

            services.AddTicketingConnector(env, config);

            services.AddSingleton<IConfigurationRoot>(i => config);

            var httpClients = new HttpClientLookup();

            if (TryGetConfigValue(config, "Services:Playbook", out string playbookBaseUrl))
            {
                httpClients.RegisterEndpoint("Playbook", playbookBaseUrl);
            }

            services.AddSingleton(httpClients);
        }

        private static bool TryGetConfigValue(this IConfigurationRoot config, string configName, out string configValue)
        {
            ThrowIf.NullOrWhiteSpace(configName, nameof(configName));
            configValue = config[configName];
            return !string.IsNullOrEmpty(configValue);
        }

        private static void ConfigureAuth(IServiceCollection services, IConfigurationRoot config)
        {
            var incidentAuthConfig = new AzureActiveDirectoryAuthenticationInfo(config["Playbook:ClientId"], config["ClientId"], config["ClientSecret"], config["AzureAd:Tenant"]);
            services.AddSingleton<AzureActiveDirectoryAuthenticationInfo>(i => incidentAuthConfig);
        }

        public static void AddThirdPartyServices(this IServiceCollection services, IConfigurationRoot config)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(iFactory
                    => new UrlHelper(iFactory.GetService<IActionContextAccessor>().ActionContext)
                );

            services.AddMvc(options =>
            {
                options.OutputFormatters.Insert(0, new PartialSerializedJsonOutputFormatter(
                        new MvcJsonOptions().SerializerSettings,
                        ArrayPool<char>.Shared));
            });
            services
                .AddAuthentication(authOptions =>
                {
                    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.Authority = String.Format(config["AzureAd:AadInstance"], config["AzureAd:Tenant"]);
                    jwtOptions.Audience = config["Frontend:ClientId"];
                    jwtOptions.SaveToken = true;
                });
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddCors();
            services.AddSockets();
            services.AddSignalR(config);
            services.AddScoped<HubConnectionBuilder>();

            //Adds every request type in the Sia.Gateway assembly
            services.AddMediatR(typeof(GetIncidentRequest).GetTypeInfo().Assembly);
            services.AddPipelineBehavior();
        }

        private static IServiceCollection AddSignalR(this IServiceCollection services, IConfigurationRoot config)
        {
            var signalRBuilder = services.AddSignalR();
            if (config.TryGetConfigValue("Redis:CacheEndpoint", out string cacheEndpoint)
                && config.TryGetConfigValue("Redis:Password", out string cachePassword))
            {
                signalRBuilder.AddRedis(redisOptions =>
                {
                    redisOptions.Options.EndPoints.Add(cacheEndpoint);
                    redisOptions.Options.Ssl = true;
                    redisOptions.Options.Password = cachePassword;
                });
            }

            return services;
        }

        private static void AddPipelineBehavior(this IServiceCollection services)
        {
            GetEventTypesShortCircuit.RegisterMe(services);
            GetEventTypeShortCircuit.RegisterMe(services);
            GetGlobalActionsShortCircuit.RegisterMe(services);
        }

       
    }
}