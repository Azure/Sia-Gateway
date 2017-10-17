using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Shared.Data
{
    public interface IHasJsonDataString
    {
        string Data { get; set; }
    }

    public interface IHasJsonDataObject
    {
        object Data { get; set; }
    }

    public class ResolveJsonToString<TSource, TDestination>
        : IValueResolver<TSource, TDestination, string>
        where TSource: IHasJsonDataObject
        where TDestination: IHasJsonDataString
    {
        public string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context)
            => JsonConvert.SerializeObject(source.Data);
    }

    public class ResolveStringToJson<TSource, TDestination>
        : IValueResolver<TSource, TDestination, object>
        where TSource : IHasJsonDataString
        where TDestination : IHasJsonDataObject
    {
        public object Resolve(TSource source, TDestination destination, object destMember, ResolutionContext context)
            => JsonConvert.DeserializeObject(source.Data);
    }
}
