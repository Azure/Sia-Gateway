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
using Sia.Shared.Authentication;
using Sia.Shared.Validation;
using System;
using System.Reflection;
using System.Runtime.Loader;
using Sia.Domain;

namespace Sia.Gateway.Initialization
{
    public static class ServicesStartup
    {

        public static void AddFirstPartyServices(this IServiceCollection services, IHostingEnvironment env, IConfigurationRoot config)
        {
            ConfigureAuth(services, config);


            if (env.IsDevelopment()) services.AddDbContext<IncidentContext>(options => options.UseInMemoryDatabase("Live"));
            if (env.IsStaging()) services.AddDbContext<IncidentContext>(options => options.UseSqlServer(config.GetConnectionString("incidentStaging")));

            services.AddTicketingConnector(env, config);

            services.AddSingleton<IConfigurationRoot>(i => config);
        }

        private static void AddTicketingConnector(this IServiceCollection services, IHostingEnvironment env, IConfigurationRoot config)
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

        private static bool TryGetConfigValue(this IConfigurationRoot config, string configName, out string configValue)
        {
            ThrowIf.NullOrWhiteSpace(configName, nameof(configName));
            configValue = config[configName];
            return !string.IsNullOrEmpty(configValue);
        }

        private static void AddProxyConnector(IServiceCollection services, IConfigurationRoot config, string proxyEndpoint)
        {
            services.AddIncidentClient(typeof(ProxyTicket));
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

        private static void ConfigureAuth(IServiceCollection services, IConfigurationRoot config)
        {
            var incidentAuthConfig = new AzureActiveDirectoryAuthenticationInfo(config["Incident:ClientId"], config["Incident:ClientSecret"], config["AzureAd:Tenant"]);
            services.AddSingleton<AzureActiveDirectoryAuthenticationInfo>(i => incidentAuthConfig);
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
            var handlerType = typeof(GetIncidentHandler<>).MakeGenericType(new Type[] { ticketType });
            services.AddScoped(typeof(IGetIncidentHandler), handlerType);
            services.AddScoped<IAsyncRequestHandler<GetIncidentRequest, Incident>, GetIncidentHandlerWrapper>();
        }

        public static void AddThirdPartyServices(this IServiceCollection services, IConfigurationRoot config)
        {
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
            services.AddSignalR(config);
            services.AddScoped<HubConnectionBuilder>();

            //Adds every request type in the Sia.Gateway assembly
            services.AddMediatR(typeof(GetIncidentRequest).GetTypeInfo().Assembly);
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

       
    }
}