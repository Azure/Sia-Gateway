using Microsoft.Extensions.DependencyInjection;
using Sia.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Initialization.Configuration
{
    public class MicroservicesConfig : IInjectableConfig
    {
        /// <summary>
        /// Endpoint for the Playbook microservice.
        /// Should be set in user secrets or environment variables.
        /// </summary>
        public string Playbook { get; set; }

        public IServiceCollection RegisterMe(IServiceCollection services)
            => services.AddSingleton(this);
    }
}
