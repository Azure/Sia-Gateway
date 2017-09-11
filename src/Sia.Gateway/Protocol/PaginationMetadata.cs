using Sia.Gateway.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Gateway.Protocol
{
    public class PaginationMetadata
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}

namespace System.Linq
{
    public static class PaginationExtensions
    {
        public static IQueryable<T> WithPagination<T>(this IQueryable<T> source, PaginationMetadata pagination)
        {
            return source.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
        }
    }
}
