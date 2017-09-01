using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Sia.Gateway.Middleware;
using System;

namespace Sia.Gateway.Initialization
{
    public static class MiddlewareStartup
    {
        public const string defaultAuthScheme = "Bearer";            

        public static void AddMiddleware(this IApplicationBuilder app, IHostingEnvironment env, IConfigurationRoot configuration)
        {
            app.UseSession();

            app.UseJwtBearerAuthentication(_jwtOptions(configuration));

            if (env.IsDevelopment() || env.IsStaging()) app.UseDeveloperExceptionPage();
            app.UseMiddleware<ExceptionHandler>();

            app.UseCors(builder =>
                builder
                    .WithOrigins(new string[]{ configuration["CORS:AcceptableOrigin"] })
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                );

            app.UseMvc();
        }

        private static JwtBearerOptions _jwtOptions(IConfigurationRoot configuration) => new JwtBearerOptions
        {
            Authority = String.Format(configuration["AzureAd:AadInstance"], configuration["AzureAd:Tenant"]),
            AuthenticationScheme = defaultAuthScheme,
            Audience = configuration["Frontend:ClientId"],
            AutomaticAuthenticate = true
        };

    }
}
