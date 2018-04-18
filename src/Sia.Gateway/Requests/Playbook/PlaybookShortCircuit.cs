using Microsoft.Extensions.Configuration;
using Sia.Gateway.Initialization.Configuration;
using Sia.Core.Extensions.Mediatr;
using Sia.Core.Requests;
using Sia.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Requests.Playbook
{
    public abstract class PlaybookShortCircuit<TRequest, TResponse> : HandlerShortCircuit<TRequest, TResponse, MicroservicesConfig>
         where TRequest : AuthenticatedRequest<TResponse>

    {
        protected PlaybookShortCircuit(MicroservicesConfig config) : base(config)
        {
        }

        public override bool ShouldRequestContinue(MicroservicesConfig config)
            => IsPlaybookMicroserviceAvailable(config);

        private static bool IsPlaybookMicroserviceAvailable(MicroservicesConfig config)
            => !String.IsNullOrWhiteSpace(config?.Playbook);
    }
}
