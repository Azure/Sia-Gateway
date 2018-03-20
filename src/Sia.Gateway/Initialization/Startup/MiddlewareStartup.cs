using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sia.Gateway.Hubs;
using Sia.Gateway.Initialization.Configuration;
using Sia.Shared.Middleware;
using System;
using System.Collections.Generic;

namespace Sia.Gateway.Initialization
{
    public static class MiddlewareStartup
    {
        public static void AddMiddleware(this IApplicationBuilder app, IHostingEnvironment env, GatewayConfiguration configuration)
        {
            app.UseAuthentication();
            app.UseSession();

            app.UseCors(builder =>
                builder
                .WithOrigins(configuration.Cors.AcceptableOrigins.ToArray())
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
            );

            app.UseSignalR(routes => 
            {
                routes.MapHub<EventsHub>(EventsHub.HubPath);
            });

            if (env.IsDevelopment() || env.IsStaging()) app.UseDeveloperExceptionPage();
            app.UseMiddleware<ExceptionHandler>();

            app.UseMvc();
        }
    }
}
