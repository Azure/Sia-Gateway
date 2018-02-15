using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Gateway.Tests
{
    [TestClass]
    public class FailingTestForTravisConfirmation
    {
        [TestMethod]
        public void FailSoTravisBuildFails()
        {
            //This only exists temporarily to make sure that 
            //Travis accurately reports failing tests
            Assert.AreEqual(true, false);
        }
    }
}
