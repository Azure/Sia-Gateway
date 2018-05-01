using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sia.Data.Incidents.Models;
using Sia.Gateway.Filters;
using System;
using System.Collections.Generic;


namespace Sia.Gateway.Tests.Filters
{
    [TestClass]
    public class EventFiltersTests
    {
        [TestMethod]
        public void EventFilterIsMatchFor_WhenFilterIsEmpty_MatchesAnyEvent()
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
        public void EventFilter_WhenProvidedOnlyIncidentId_IsMatchForEventWithSameIncidentId()
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
        public void EventFilter_WhenProvidedOnlyIncidentId_IsNotMatchForEventWithDifferentIncidentId()
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
        public void Filter_WhenProvidedOnlyEventTypes_IsMatchForEventWithIncludedEventType()
        {
            var serviceUnderTest = new EventFilters();
            serviceUnderTest.EventTypes.Add(1);
            serviceUnderTest.EventTypes.Add(3);
            var testInput = new Event()
            {
                EventTypeId = 1
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Filter_WhenProvidedOnlyEventTypes_IsNotMatchForEventWithNotIncludedEventType()
        {
            var serviceUnderTest = new EventFilters();
            serviceUnderTest.EventTypes.Add(1);
            serviceUnderTest.EventTypes.Add(3);
            var testInput = new Event()
            {
                EventTypeId = 2
            };

            var result = serviceUnderTest.IsMatchFor(testInput);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EventFilter_WhenProvidedOnlyStartTime_IsMatchForEventWithOccurredTimeAfterStartTime()
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
        public void EventFilter_WhenProvidedOnlyStartTime_IsNotMatchForEventWithOccurredTimeBeforeStartTime()
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
        public void EventFilter_WhenProvidedOnlyEndTime_IsMatchForEventWithOccurredTimeBeforeEndTime()
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
        public void EventFilter_WhenProvidedOnlyEndTime_IsNotMatchForEventWithOccurredTimeAfterEndTime()
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
        public void EventFilter_WhenProvidedOnlyDataKey_IsMatchForEventWithMatchingKeyInData()
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
        public void EventFilter_WhenProvidedOnlyDataKey_IsNotMatchForEventWithoutMatchingKeyInData()
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
        public void EventFilter_WhenProvidedDataKeyAndDataValue_IsMatchForEventWithMatchingKeyAndValueInData()
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
        public void EventFilter_WhenProvidedDataKeyAndDataValue_IsNotMatchForEventWithMatchingKeyAndNonMatchingValueInData()
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
