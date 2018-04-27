using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.State.Configuration;
using Sia.State.Configuration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.State.Tests.Configuration
{
    [TestClass]
    public class LoadConfigurationFromGithubTests
    {
        [TestMethod]
        public void ToCombinedReducerConfiguration_WhenProvidedWellFormedPathHierarchy_ReturnsCombinedReducerConfigurationBasedOnHierarchy()
        {
            var mockPathHierarchy = new List<(string[] pathTokens, ReducerConfiguration reducerConfig)>()
            {
                (pathTokens: new string[] { "first", "second", "third"}, new ReducerConfiguration()),
                (pathTokens: new string[] { "first", "second", "altthird"}, new ReducerConfiguration()),
                (pathTokens: new string[] { "first", "second"}, new ReducerConfiguration()),
                (pathTokens: new string[] { "first", "altsecond"}, new ReducerConfiguration()),
                (pathTokens: new string[] { "first", "altsecond", "third"}, new ReducerConfiguration()),
                (pathTokens: new string[] { "first" }, new ReducerConfiguration()),
                (pathTokens: new string[] { "altfirst" }, new ReducerConfiguration()),
            }.GroupBy(tokensToReducer => tokensToReducer.pathTokens.Count());

            var result = mockPathHierarchy.ToCombinedReducerConfiguration();

            Assert.AreEqual(2, result.SimpleChildren.Count);
            Assert.AreEqual(1, result.CompositeChildren.Count);
            Assert.IsTrue(result.CompositeChildren.TryGetValue("first", out var firstComplexChild));
            Assert.AreEqual(2, firstComplexChild.SimpleChildren.Count);
            Assert.AreEqual(2, firstComplexChild.CompositeChildren.Count);
            Assert.IsTrue(firstComplexChild.CompositeChildren.TryGetValue("second", out var secondComplexChild));
            Assert.AreEqual(2, secondComplexChild.SimpleChildren.Count);
            Assert.AreEqual(0, secondComplexChild.CompositeChildren.Count);
        }
    }
}
