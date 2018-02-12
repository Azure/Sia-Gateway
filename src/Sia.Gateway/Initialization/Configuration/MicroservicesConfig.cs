using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Initialization.Configuration
{
    public class MicroservicesConfig
    {
        /// <summary>
        /// Endpoint for the Playbook microservice.
        /// Should be set in user secrets or environment variables.
        /// </summary>
        public string Playbook { get; set; }
    }
}
