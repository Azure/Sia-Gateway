using Microsoft.AspNetCore.Mvc;
using Sia.Core.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Gateway.Links
{
    public abstract class LinksProvider
    {
        protected IUrlHelper UrlHelper { get; }
        protected LinksProvider(IUrlHelper urlHelper)
        {
            UrlHelper = urlHelper;
        }
        protected abstract OperationLinks GetOperationLinks(object routeValues);
    }
}
