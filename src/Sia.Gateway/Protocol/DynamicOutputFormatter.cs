﻿using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Buffers;
using System.Text;
using Sia.Shared.Data;

namespace Sia.Gateway.Protocol
{
    public class DynamicOutputFormatter : JsonOutputFormatter
    {
        public DynamicOutputFormatter(JsonSerializerSettings serializerSettings, ArrayPool<char> charPool)
            : base(serializerSettings, charPool)
        {

        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var dataStream = (IEnumerable<IDynamicDataSource>)context.Object;

            foreach (var objectToWrite in dataStream)
            {
                var dynamicData = objectToWrite.Data;
                if (dynamicData is string) objectToWrite.Data = Deserialize((string)dynamicData);
            }

            return base.WriteResponseBodyAsync(context, selectedEncoding);
        }

        private const int NumberOfCharactersInGenericTypeNotUsedByGetInterfaceMethod = 3;

        protected override bool CanWriteType(Type type)
        {
            if (!type.IsGenericType) return false;
            if (type.GetGenericArguments().Count() != 1) return false;

            var enumIntName = typeof(IEnumerable<>).ToString();
            var enumerableInterface = type.GetInterface(enumIntName
                .Substring(0, enumIntName.Length - NumberOfCharactersInGenericTypeNotUsedByGetInterfaceMethod));
            if (enumerableInterface is null) return false;

            return !(type.GetGenericArguments()[0].GetInterface(nameof(IDynamicDataSource)) is null);
        }

        private object Deserialize(string serializedData) => JsonConvert.DeserializeObject(serializedData);
    }
}
