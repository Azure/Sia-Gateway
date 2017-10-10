using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sia.Connectors.Tickets.None;
using Sia.Connectors.Tickets.TicketProxy;
using Sia.Data.Incidents;
using Sia.Gateway.Authentication;
using Sia.Gateway.Requests;
using Sia.Gateway.ServiceRepositories;
using Sia.Shared.Authentication;
using Sia.Shared.Validation;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Runtime.Loader;

namespace Sia.Gateway.Initialization
{
    public static class ServicesStartup
    {

        public static void AddFirstPartyServices(this IServiceCollection services, IHostingEnvironment env, IConfigurationRoot config)
        {

            var incidentAuthConfig = new AzureActiveDirectoryAuthenticationInfo(config["Incident:ClientId"], config["Incident:ClientSecret"], config["AzureAd:Tenant"]);

            if (env.IsDevelopment()) services.AddDbContext<IncidentContext>(options => options.UseInMemoryDatabase("Live"));
            if (env.IsStaging()) services.AddDbContext<IncidentContext>(options => options.UseSqlServer(config.GetConnectionString("incidentStaging")));

            AddTicketingConnector(services, env, config);

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEngagementRepository, EngagementRepository>();

            services.AddSingleton<IConfigurationRoot>(i => config);
            services.AddSingleton<AzureActiveDirectoryAuthenticationInfo>(i => incidentAuthConfig);
        }

        private static void AddTicketingConnector(IServiceCollection services, IHostingEnvironment env, IConfigurationRoot config)
        {
            if (TryGetConfigValue(config, "Connector:Ticket:Path", out var ticketConnectorAssemblyPath))
            {
                LoadConnectorFromAssembly(services, env, config, ticketConnectorAssemblyPath);
                return;
            }

            if (TryGetConfigValue(config, "Connector:Ticket:ProxyEndpoint", out var proxyEndpoint))
            {
                AddProxyConnector(services, config, proxyEndpoint);
                return;
            }

            services.AddIncidentClient(typeof(EmptyTicket));
            services.AddNoTicketingSystem();
        }

        private static bool TryGetConfigValue(IConfigurationRoot config, string configName, out string configValue)
        {
            ThrowIf.NullOrWhiteSpace(configName, nameof(configName));
            configValue = config[configName];
            return !string.IsNullOrEmpty(configValue);
        }

        private static void AddProxyConnector(IServiceCollection services, IConfigurationRoot config, string proxyEndpoint)
        {
            services.AddIncidentClient(typeof(Ticket));
            var proxyAuthType = config["Connector:Ticket:ProxyAuthType"];
            switch(proxyAuthType)
            {
                case "Certificate":
                    services.AddProxyWithCert(proxyEndpoint, config["Connector:Ticket:ProxyCertThumbprint"]);
                    return;
                case "VaultCertificate":
                    services.AddProxyWithCertFromKeyVault(
                        proxyEndpoint,
                        new KeyVaultConfiguration(
                            config["ClientId"],
                            config["ClientSecret"],
                            config["Connector:Ticket:VaultName"]
                        ),
                        config["Connector:Ticket:CertName"]
                    );
                    return;
                default:
                    services.AddProxyWithoutAuth(proxyEndpoint);
                    return;
            }
        }

        private static void LoadConnectorFromAssembly(IServiceCollection services, IHostingEnvironment env, IConfigurationRoot config, string ticketConnectorAssemblyPath)
        {
            var connectorAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(ticketConnectorAssemblyPath);
            var connectorInitializerType = connectorAssembly.GetType("Sia.Gateway.Initialization.Initialization");

            var ticketType = (Type)connectorInitializerType.GetMethod("TicketType").Invoke(null, null);
            services.AddIncidentClient(ticketType);

            var connectorConfigureServices = connectorInitializerType.GetMethod("AddConnector", new Type[] { typeof(IServiceCollection), typeof(IConfigurationRoot), typeof(IHostingEnvironment) });
            connectorConfigureServices.Invoke(null, new object[] { services, config, env });
        }

        private static void AddIncidentClient(this IServiceCollection services, Type ticketType)
        {
            var clientType = typeof(IncidentRepository<>).MakeGenericType(new Type[] { ticketType });
            services.AddScoped(typeof(IIncidentRepository), clientType);
        }

        public static void AddThirdPartyServices(this IServiceCollection services, IConfigurationRoot config)
        {
            //Adds every request type in the Sia.Gateway assembly
            services.AddMediatR(typeof(GetIncidentRequest).GetTypeInfo().Assembly);

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(iFactory
                    => new UrlHelper(iFactory.GetService<IActionContextAccessor>().ActionContext)
                );

            services.AddMvc();
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
            services.AddSignalR().AddRedis(redisOptions =>
            {
                redisOptions.Options.EndPoints.Add(config["Redis:CacheEndpoint"]);
                redisOptions.Options.Ssl = true;
                redisOptions.Options.Password = config["Redis:Password"];
            });
            services.AddScoped<HubConnectionBuilder>();
        }
    }
}