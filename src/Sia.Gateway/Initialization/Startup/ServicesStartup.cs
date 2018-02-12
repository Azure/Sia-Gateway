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
using Microsoft.Extensions.Primitives;
using Sia.Connectors.Tickets.None;
using Sia.Connectors.Tickets.TicketProxy;
using Sia.Data.Incidents;
using Sia.Domain;
using Sia.Gateway.Hubs;
using Sia.Gateway.Initialization.Configuration;
using Sia.Gateway.Requests;
using Sia.Gateway.Requests.Playbook;
using Sia.Shared.Authentication;
using Sia.Shared.Authentication.Http;
using Sia.Shared.Configuration;
using Sia.Shared.Protocol;
using Sia.Shared.Validation;
using System;
using System.Buffers;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace Sia.Gateway.Initialization
{
    public static class ServicesStartup
    {

        public static void AddFirstPartyServices(
            this IServiceCollection services,
            IHostingEnvironment env,
            IConfigurationRoot rawConfig,
            GatewayConfiguration config)
        {
            ConfigureAuth(services, config);


            if (env.IsDevelopment()) services.AddDbContext<IncidentContext>(options => options.UseInMemoryDatabase("Live"));
            if (env.IsStaging()) services.AddDbContext<IncidentContext>(options => options.UseSqlServer(rawConfig.GetConnectionString("incidentStaging")));

            services.AddTicketingConnector(env, rawConfig, config.Connector.Ticket);

            services.AddSingleton<IConfigurationRoot>(i => rawConfig);

            var httpClients = new HttpClientLookup();

            if (!String.IsNullOrEmpty(config.Services.Playbook))
            {
                httpClients.RegisterEndpoint(nameof(config.Services.Playbook), config.Services.Playbook);
            }

            services.AddSingleton(httpClients);
        }

        public static void ConfigureAuth(IServiceCollection services, GatewayConfiguration config)
        {
            var incidentAuthConfig = new AzureActiveDirectoryAuthenticationInfo(config.Playbook.ClientId, config.ClientId, config.ClientSecret, config.AzureAd.Tenant);
            services.AddSingleton<AzureActiveDirectoryAuthenticationInfo>(i => incidentAuthConfig);
        }

        public static void AddThirdPartyServices(this IServiceCollection services, GatewayConfiguration config)
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
                    jwtOptions.Authority = config.AzureAd.Authority;
                    jwtOptions.Audience = config.FrontEnd.ClientId;
                    jwtOptions.SaveToken = true;
                    jwtOptions.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Path.Value.StartsWith(string.Concat("/", EventsHub.HubPath))
                                && context.Request.Query.TryGetValue("token", out StringValues token))
                            {
                                context.Token = token;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
            services
                .AddDistributedMemoryCache()
                .AddSession()
                .AddCors()
                .AddSockets()
                .AddSignalR(config.Redis)
                .AddScoped<HubConnectionBuilder>();

            //Adds every request type in the Sia.Gateway assembly
            services.AddMediatR(typeof(GetIncidentRequest).GetTypeInfo().Assembly);
            services.AddMediatRPipelineBehavior();
        }

        public static IServiceCollection AddSignalR(this IServiceCollection services, RedisConfig config)
        {
            var signalRBuilder = services.AddSignalR();
            if (config != null && config.IsValid)
            {
                signalRBuilder.AddRedis(redisOptions =>
                {
                    redisOptions.Options.EndPoints.Add(config.CacheEndpoint);
                    redisOptions.Options.Ssl = true;
                    redisOptions.Options.Password = config.Password;
                });
            }

            return services;
        }

        public static IServiceCollection AddMediatRPipelineBehavior(this IServiceCollection services)
            => GetEventTypesShortCircuit.RegisterMe(
            GetEventTypeShortCircuit.RegisterMe(
            GetGlobalActionsShortCircuit.RegisterMe(services)));



    }
}