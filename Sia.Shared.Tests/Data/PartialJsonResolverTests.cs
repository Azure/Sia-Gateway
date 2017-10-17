using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Shared.Tests.Data
{
    

    [TestClass]
    public class PartialJsonResolverTests
    {
        [TestMethod]
        public void ResolveJsonToString_Method_Resolve_SerializesSourceArgumentObjectToString()
        {
            var input = new TestHasJsonDataObject
            {
                Data = new JsonSerializationTestObject()
            };
            var expectedResultDataValue = JsonSerializationTestObject.ExpectedSerialization();
            var objectUnderTest = new ResolveJsonToString<TestHasJsonDataObject, TestHasJsonDataString>();


            var result = objectUnderTest.Resolve(input, null, null, null);


            Assert.AreEqual(expectedResultDataValue, result, false);
        }

        [TestMethod]
        public void ResolveStringToJson_Method_Resolve_SerializesSourceArgumentStringToObject()
        {
            var input = new TestHasJsonDataString()
            {
                Data = JsonSerializationTestObject.ExpectedSerialization()
            };
            var expectedResult = new JsonSerializationTestObject();
            var objectUnderTest = new ResolveStringToJson<TestHasJsonDataString, TestHasJsonDataObject>();


            var result = objectUnderTest.Resolve(input, null, null, null);


            Assert.AreEqual(expectedResult.a, ExtractPropertyFromResult(result, "a"));
            Assert.AreEqual(expectedResult.b, ExtractPropertyFromResult(result, "b"));
        }

        private static JToken ExtractPropertyFromResult(object result, string propName) => ((JObject)result).Property(propName).Value;
    }

    internal class JsonSerializationTestObject :IEquatable<JsonSerializationTestObject>
    {
        public static string ExpectedSerialization()
            => "{\"a\":\"ValueOfA\",\"b\":1}";
        public bool Equals(JsonSerializationTestObject other)
            => a == other.a && b == other.b;

        public string a { get; set; } = "ValueOfA";
        public int b { get; set; } = 1;
    }

    internal class TestHasJsonDataString : IHasJsonDataString
    {
        public string Data { get; set; }
    }

    internal class TestHasJsonDataObject : IHasJsonDataObject
    {
        public object Data { get; set; }
    }
}
