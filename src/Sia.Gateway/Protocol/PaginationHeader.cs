using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Sia.Gateway.Protocol
{
    public class PaginationHeader
    {
        private PaginationMetadata _metadata;
        private IUrlHelper _urlHelper;
        private string _routeName;

        public PaginationHeader(PaginationMetadata metadata, IUrlHelper urlHelper, string routeName)
        {
            _metadata = metadata;
            _urlHelper = urlHelper;
            _routeName = routeName;
        }

        public string HeaderName => "X-Pagination";
        public StringValues HeaderValues => JsonConvert.SerializeObject(new
        {
            PageNumber = _metadata.PageNumber,
            PageSize = _metadata.PageSize,
            TotalRecords = _metadata.TotalRecords,
            TotalPages = _metadata.TotalPages,
            NextPageLink = _metadata.NextPageExists ? _urlHelper.Action(_routeName, _metadata.NextPageLinkInfo) : null,
            PrevPageLink = _metadata.PreviousPageExists ? _urlHelper.Action(_routeName, _metadata.PreviousPageLinkInfo) : null
        });
    }
}

namespace Microsoft.AspNetCore.Mvc
{
    using Microsoft.AspNetCore.Http;
    using Sia.Gateway.Protocol;

    public static class PaginationExtensions
    {
        public static void AddPagination(this IHeaderDictionary headers, PaginationHeader header)
        {
            headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            headers.Add(header.HeaderName, header.HeaderValues);
        }
    }
}