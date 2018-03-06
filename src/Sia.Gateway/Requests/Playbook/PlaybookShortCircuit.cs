using Microsoft.Extensions.Configuration;
using Sia.Gateway.Initialization.Configuration;
using Sia.Shared.Extensions.Mediatr;
using Sia.Shared.Requests;
using Sia.Shared.Validation;
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
