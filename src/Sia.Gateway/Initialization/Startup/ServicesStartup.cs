﻿using MediatR;
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
using Sia.Core.Authentication;
using Sia.Core.Authentication.Http;
using Sia.Core.Configuration;
using Sia.Core.Data;
using Sia.Core.Protocol;
using Sia.Core.Validation;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using System.Globalization;
using Sia.Gateway.Links;
using Sia.State.Services.Demo;

namespace Sia.Gateway.Initialization
{
    public static class ServicesStartup
    {

        public static void AddFirstPartyServices(
            this IServiceCollection services,
            IHostingEnvironment env,
            IConfigurationRoot rawConfig,
            GatewayConfiguration config)
            => services
                .RegisterConfig(config)
                .AddAuth(config)
                .AddDatabase(env, config)
                .AddTicketingConnector(env, rawConfig, config?.Connector?.Ticket)
                .AddMicroserviceProxies(config)
                .AddRouteHelpers()
                .AddReducersFromCode<DemoReducerService>();

        public static IServiceCollection AddAuth(this IServiceCollection services, GatewayConfiguration config)
        {
            var incidentAuthConfig = new AzureActiveDirectoryAuthenticationInfo(config.Playbook.ClientId, config.ClientId, config.ClientSecret, config.AzureAd.Tenant);
            return services.AddSingleton<AzureActiveDirectoryAuthenticationInfo>(i => incidentAuthConfig);
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IHostingEnvironment env, GatewayConfiguration config)
        {
            if (env.IsDevelopment())
            {
                services.AddDbContext<IncidentContext>(options => options.UseInMemoryDatabase("Live"));
            }
            else
            {
                services.AddDbContext<IncidentContext>(options => options.UseSqlServer(config.GatewayDatabaseConnectionString));
            }
      
            return services;
        }

        public static IServiceCollection AddMicroserviceProxies(this IServiceCollection services, GatewayConfiguration config)
        {
            var httpClients = new HttpClientLookup();

            if (!String.IsNullOrEmpty(config.Services.Playbook))
            {
                httpClients.RegisterEndpoint(nameof(config.Services.Playbook), config.Services.Playbook);
            }

            return services.AddSingleton(httpClients);
        }

        public static void AddThirdPartyServices(this IServiceCollection services, IHostingEnvironment env, GatewayConfiguration config)
            => services
                .AddMvcServices(env, config)
                .AddDistributedMemoryCache()
                .AddSession()
                .AddCors()
                .AddSockets()
                .AddSignalR(config.Redis)
                .AddMediatRConfig();

        public static IServiceCollection AddMvcServices(this IServiceCollection services, IHostingEnvironment env, GatewayConfiguration config)
            => services
            .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
            .AddScoped<IUrlHelper, UrlHelper>(iFactory
                    => new UrlHelper(iFactory.GetService<IActionContextAccessor>().ActionContext)
            ).AddMvc(options =>
            {
                options.OutputFormatters.Insert(0, new PartialSerializedJsonOutputFormatter(
                        new MvcJsonOptions().SerializerSettings,
                        ArrayPool<char>.Shared));
            }).Services
            .AddAuthentication(authOptions =>
            {
                authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.Authority = config.AzureAd.Authority;
                jwtOptions.Audience = config.FrontEnd.ClientId;
                jwtOptions.SaveToken = true;
                jwtOptions.RequireHttpsMetadata = !env.IsDevelopment();
                
                jwtOptions.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Path.Value.StartsWith(EventsHub.HubPath, ignoreCase: true, culture: CultureInfo.InvariantCulture)
                            && context.Request.Query.TryGetValue("access_token", out StringValues token))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    }
                };

                jwtOptions.Validate();
            }).Services;

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

            var eventFilterRegistry = new ConcurrentDictionary<string, IFilterByMatch<Event>>();

            return services
                .AddScoped<HubConnectionBuilder>()
                .AddSingleton(eventFilterRegistry);
        }

        public static IServiceCollection AddMediatRConfig(this IServiceCollection services)
        {
            //Adds every request type in the Sia.Gateway assembly
            services.AddMediatR(typeof(GetIncidentRequest).GetTypeInfo().Assembly);

            //Pipeline behavior
            GetEventTypesShortCircuit.RegisterMe(
            GetEventTypeShortCircuit.RegisterMe(
            GetGlobalActionsShortCircuit.RegisterMe(services)));
            return services;
        }

        public static IServiceCollection AddRouteHelpers(this IServiceCollection services)
            => services
                .AddScoped<IncidentLinksProvider>()
                .AddScoped<EventLinksProvider>();

    }
}