using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Shared.Data
{
    public interface IDynamicDataStorage
    {
        string Data { get; set; }
    }

    public interface IDynamicDataSource
    {
        dynamic Data { get; set; }
    }

    public class ResolveFromDynamic<TSource, TDestination>
        : IValueResolver<TSource, TDestination, string>
        where TSource: IDynamicDataSource
        where TDestination: IDynamicDataStorage
    {
        public string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context)
            => JsonConvert.SerializeObject(source.Data);

    }

    public class ResolveToDynamic<TSource, TDestination>
        : IValueResolver<TSource, TDestination, object>
        where TSource : IDynamicDataStorage
        where TDestination : IDynamicDataSource
    {
        public object Resolve(TSource source, TDestination destination, object destMember, ResolutionContext context)
            => JsonConvert.DeserializeObject<ExpandoObject>(source.Data);
    }
}
