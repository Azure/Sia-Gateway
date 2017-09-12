using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        protected readonly AuthenticatedUserContext _authContext;

        protected BaseController(IMediator mediator, AzureActiveDirectoryAuthenticationInfo authConfig)
        {
            _mediator = mediator;
            _authConfig = authConfig;
            _authContext = new AuthenticatedUserContext(User, HttpContext.Session, _authConfig);
        }

    }
}
