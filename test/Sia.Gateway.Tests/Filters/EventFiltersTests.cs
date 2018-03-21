using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sia.Data.Incidents.Models;
using Sia.Gateway.Filters;
using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Gateway.Tests.Filters
{
    [TestClass]
    public class EventFiltersTests
    {
        [TestMethod]
        public void EventFilter_IsMatchFor_WhenFilterIsEmpty_MatchesAnyEvent()
        {
            var serviceUnderTest = new EventFilters();
            var testInput = new Event()
            {
                IncidentId = 1,
                EventTypeId = 1,
                Occurred = new DateTime(1970, 1, 1),
                EventFired = new DateTime(1970, 1, 1),
                Data = "firstExpectedEvent"
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EventFilter_WhenProvidedOnlyIncidentId_IsMatchFor_EventWithSameIncidentId()
        {
            var serviceUnderTest = new EventFilters()
            {
                IncidentId = 1
            };
            var testInput = new Event()
            {
                IncidentId = 1
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EventFilter_WhenProvidedOnlyIncidentId_IsNotMatchFor_EventWithDifferentIncidentId()
        {
            var serviceUnderTest = new EventFilters()
            {
                IncidentId = 1
            };
            var testInput = new Event()
            {
                IncidentId = 2
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Filter_WhenProvidedOnlyEventTypes_IsMatchFor_EventWithIncludedEventType()
        {
            var serviceUnderTest = new EventFilters()
            {
                EventTypes = new long[] { 1, 3 }
            };
            var testInput = new Event()
            {
                EventTypeId = 1
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Filter_WhenProvidedOnlyEventTypes_IsNotMatchFor_EventWithNotIncludedEventType()
        {
            var serviceUnderTest = new EventFilters()
            {
                EventTypes = new long[] { 1, 3 }
            };
            var testInput = new Event()
            {
                EventTypeId = 2
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EventFilter_WhenProvidedOnlyStartTime_IsMatchFor_EventWithOccurredTimeAfterStartTime()
        {
            var serviceUnderTest = new EventFilters()
            {
                StartTime = new DateTime(1970, 1, 1)
            };
            var testInput = new Event()
            {
                Occurred = new DateTime(1971, 1, 1),
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EventFilter_WhenProvidedOnlyStartTime_IsNotMatchFor_EventWithOccurredTimeBeforeStartTime()
        {
            var serviceUnderTest = new EventFilters()
            {
                StartTime = new DateTime(1972, 1, 1)
            };
            var testInput = new Event()
            {
                Occurred = new DateTime(1971, 1, 1),
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EventFilter_WhenProvidedOnlyEndTime_IsMatchFor_EventWithOccurredTimeBeforeEndTime()
        {
            var serviceUnderTest = new EventFilters()
            {
                EndTime = new DateTime(1972, 1, 1)
            };
            var testInput = new Event()
            {
                Occurred = new DateTime(1971, 1, 1),
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EventFilter_WhenProvidedOnlyEndTime_IsNotMatchFor_EventWithOccurredTimeAfterEndTime()
        {
            var serviceUnderTest = new EventFilters()
            {
                EndTime = new DateTime(1970, 1, 1)
            };
            var testInput = new Event()
            {
                Occurred = new DateTime(1971, 1, 1),
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EventFilter_WhenProvidedOnlyDataKey_IsMatchFor_EventWithMatchingKeyInData()
        {
            var serviceUnderTest = new EventFilters()
            {
                DataKey = "ExpectedKey"
            };
            var testInput = new Event()
            {
                Data = JsonConvert.SerializeObject(new
                {
                    IrrelevantProperty = "HelloWorld",
                    ExpectedKey = "IrrelevantValue",
                    AnotherIrrelevantProperty = "IrrelevantValue"
                })
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EventFilter_WhenProvidedOnlyDataKey_IsNotMatchFor_EventWithoutMatchingKeyInData()
        {
            var serviceUnderTest = new EventFilters()
            {
                DataKey = "ExpectedKey"
            };
            var testInput = new Event()
            {
                Data = JsonConvert.SerializeObject(new
                {
                    IrrelevantProperty = "HelloWorld",
                    UnexpectedKey = "IrrelevantValue",
                    AnotherIrrelevantProperty = "IrrelevantValue"
                })
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EventFilter_WhenProvidedDataKeyAndDataValue_IsMatchFor_EventWithMatchingKeyAndValueInData()
        {
            var serviceUnderTest = new EventFilters()
            {
                DataKey = "ExpectedKey",
                DataValue = "ExpectedValue"
            };
            var testInput = new Event()
            {
                Data = JsonConvert.SerializeObject(new
                {
                    IrrelevantProperty = "HelloWorld",
                    ExpectedKey = "ExpectedValue",
                    AnotherIrrelevantProperty = "IrrelevantValue"
                })
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EventFilter_WhenProvidedDataKeyAndDataValue_IsNotMatchFor_EventWithMatchingKeyAndNonMatchingValueInData()
        {
            var serviceUnderTest = new EventFilters()
            {
                DataKey = "ExpectedKey",
                DataValue = "ExpectedValue"
            };
            var testInput = new Event()
            {
                Data = JsonConvert.SerializeObject(new
                {
                    IrrelevantProperty = "HelloWorld",
                    ExpectedKey = "UnexpectedValue",
                    AnotherIrrelevantProperty = "IrrelevantValue"
                })
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsFalse(result);
        }
    }
}
