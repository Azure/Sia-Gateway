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
        public void EventFilterIsMatchForWhenFilterIsEmptyMatchesAnyEvent()
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
        public void EventFilterWhenProvidedOnlyIncidentIdIsMatchForEventWithSameIncidentId()
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
        public void EventFilterWhenProvidedOnlyIncidentIdIsNotMatchForEventWithDifferentIncidentId()
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
        public void FilterWhenProvidedOnlyEventTypesIsMatchForEventWithIncludedEventType()
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
        public void FilterWhenProvidedOnlyEventTypesIsNotMatchForEventWithNotIncludedEventType()
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
        public void EventFilterWhenProvidedOnlyStartTimeIsMatchForEventWithOccurredTimeAfterStartTime()
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
        public void EventFilterWhenProvidedOnlyStartTimeIsNotMatchForEventWithOccurredTimeBeforeStartTime()
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
        public void EventFilterWhenProvidedOnlyEndTimeIsMatchForEventWithOccurredTimeBeforeEndTime()
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
        public void EventFilterWhenProvidedOnlyEndTimeIsNotMatchForEventWithOccurredTimeAfterEndTime()
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
        public void EventFilterWhenProvidedOnlyDataKeyIsMatchForEventWithMatchingKeyInData()
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
        public void EventFilterWhenProvidedOnlyDataKeyIsNotMatchForEventWithoutMatchingKeyInData()
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
        public void EventFilterWhenProvidedDataKeyAndDataValueIsMatchForEventWithMatchingKeyAndValueInData()
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
        public void EventFilterWhenProvidedDataKeyAndDataValueIsNotMatchForEventWithMatchingKeyAndNonMatchingValueInData()
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
