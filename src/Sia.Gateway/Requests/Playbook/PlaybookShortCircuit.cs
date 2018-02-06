using Microsoft.Extensions.Configuration;
using Sia.Shared.Extensions.Mediatr;
using Sia.Shared.Requests;
using Sia.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests.Playbook
{
    public abstract class PlaybookShortCircuit<TRequest, TResponse> : HandlerShortCircuit<TRequest, TResponse>
         where TRequest : AuthenticatedRequest<TResponse>

    {
        private const string _configSectionTemplate = "Microservices:{0}";
        private const string _ourMicroservice = "Playbook";
        protected PlaybookShortCircuit(IConfigurationRoot config) : base(config)
        {
        }

        public override bool ShouldRequestContinue(IConfigurationRoot config)
        => IsPlaybookMicroserviceAvailable(config);

        private static bool IsPlaybookMicroserviceAvailable(IConfigurationRoot config)
        {
            int i = 0;
            string currentMicroservice;
            do
            {
                var key = String.Format(_configSectionTemplate, i.ToString());
                currentMicroservice = config[key];
                if (currentMicroservice == _ourMicroservice)
                {
                    return true;
                }
                i++;
            }
            while (currentMicroservice != null);
            return false;
        }
    }
}
