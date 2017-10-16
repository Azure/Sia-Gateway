using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sia.Gateway.Authentication;
using Sia.Gateway.Validation.Filters;


namespace Sia.Gateway.Controllers
{
    [Return400BadRequestWhenModelStateInvalid]
    [Authorize()]
    public abstract class BaseController : Controller
    {
        protected readonly IMediator _mediator;
        protected readonly AzureActiveDirectoryAuthenticationInfo _authConfig;
        protected readonly IUrlHelper _urlHelper;
        protected readonly ILoggerFactory _loggerFactory;

        protected AuthenticatedUserContext _authContext => new AuthenticatedUserContext(User, HttpContext.Session, _authConfig);

        protected BaseController(IMediator mediator, 
            AzureActiveDirectoryAuthenticationInfo authConfig,
            ILoggerFactory loggerFactory,
            IUrlHelper urlHelper)
        {
            _mediator = mediator;
            _authConfig = authConfig;
            _urlHelper = urlHelper;
            _loggerFactory = loggerFactory;
        }

    }
}
