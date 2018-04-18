// TODO remove unused usings.

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sia.Domain.ApiModels;
using Sia.Core.Authentication;
using Sia.Gateway.Requests;
using System.Threading.Tasks;
using Sia.Core.Controllers;
using System;

namespace Sia.Gateway.Controllers
{
  [Authorize]
  [Route("/about")]
  public class AboutController : Controller
  {
    public AboutController()
        : base() { }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
    }
  }
}
