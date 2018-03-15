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
        public void Filter_WhenPassedEmptyQueryable_ReturnsEmptyQueryable()
        {
            var serviceUnderTest = new EventFilters();
            var testInput = new List<Event>().AsQueryable();


            var result = testInput.WithFilter(serviceUnderTest);


            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void Filter_WhenFilterIsEmpty_ReturnsInputQueryable()
        {
            var serviceUnderTest = new EventFilters();
            var testInput = new List<Event>()
            {
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = 1,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1),
                    Data = "firstExpectedEvent"
                },
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = 1,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1),
                    Data = "secondExpectedEvent"
                }
            }.AsQueryable();


            var result = testInput.WithFilter(serviceUnderTest);


            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void Filter_WhenPassedQueryable_ReturnsOnlyMatchingResultsAsQueryable()
        {
            var serviceUnderTest = new EventFilters()
            {
                IncidentId = 1,
                EventTypes = new long[] { 1 },
                Occurred = new DateTime(1970, 1, 1),
                EventFired = new DateTime(1970, 1, 1)
            };
            var testInput = new List<Event>()
            {
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = 1,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1),
                    Data = "firstExpectedEvent"
                },
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = 1,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1),
                    Data = "secondExpectedEvent"
                },
                new Event()
                {
                    IncidentId = 2,
                    EventTypeId = 1,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1)
                },
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = 2,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1)
                },
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = 1,
                    Occurred = new DateTime(1972, 2, 2),
                    EventFired = new DateTime(1970, 1, 1)
                },
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = 1,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1972, 2, 2)
                },
            }.AsQueryable();


            var result = testInput.WithFilter(serviceUnderTest);


            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Select(ev => ev.Data).Contains("firstExpectedEvent"));
            Assert.IsTrue(result.Select(ev => ev.Data).Contains("secondExpectedEvent"));
        }

        [TestMethod]
        public void Filter_WhenPassedQueryable_MatchesByDataKeyWhenEventsHaveEquivalentKeyInData()
        {
            var serviceUnderTest = new EventFilters()
            {
                DataKey = "HelloWorld"
            };
            var expectedEventTypeId = 2;
            var unexpectedEventTypeId = 1;
            var testInput = new List<Event>()
            {
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = unexpectedEventTypeId,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1),
                    Data = "NotMatched"
                },
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = expectedEventTypeId,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1),
                    Data = JsonConvert.SerializeObject(new
                    {
                        IrrelevantProperty = "IrrelevantValue",
                        HelloWorld = "IrrelevantValue",
                        AnotherIrrelevantProperty = "IrrelevantValue"
                    })
                }
            }.AsQueryable();


            var result = testInput.WithFilter(serviceUnderTest);


            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(expectedEventTypeId, result.First().EventTypeId);
        }

        [TestMethod]
        public void Filter_WhenPassedQueryable_ReturnsEmptyQueryableWhenNoEventsHaveEquivalentKeyInData()
        {
            var serviceUnderTest = new EventFilters()
            {
                DataKey = "HelloWorld"
            };
            var expectedEventTypeId = 2;
            var unexpectedEventTypeId = 1;
            var testInput = new List<Event>()
            {
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = unexpectedEventTypeId,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1),
                    Data = "NotMatched"
                },
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = expectedEventTypeId,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1),
                    Data = JsonConvert.SerializeObject(new
                    {
                        IrrelevantProperty = "IrrelevantValue",
                        HelloBob = "IrrelevantValue",
                        AnotherIrrelevantProperty = "IrrelevantValue"
                    })
                }
            }.AsQueryable();


            var result = testInput.WithFilter(serviceUnderTest);


            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Filter_WhenPassedQueryable_MatchesByDataKeyAndValueWhenEventsHaveEquivalentKeyAndValueInData()
        {
            var serviceUnderTest = new EventFilters()
            {
                DataKey = "HelloWorld",
                DataValue = "HelloWorld"
            };
            var expectedEventTypeId = 2;
            var unexpectedEventTypeId = 1;
            var testInput = new List<Event>()
            {
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = unexpectedEventTypeId,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1),
                    Data = "NotMatched"
                },
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = expectedEventTypeId,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1),
                    Data = JsonConvert.SerializeObject(new
                    {
                        IrrelevantProperty = "HelloWorld",
                        HelloWorld = "HelloWorld",
                        AnotherIrrelevantProperty = "IrrelevantValue"
                    })
                }
            }.AsQueryable();


            var result = testInput.WithFilter(serviceUnderTest);


            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(expectedEventTypeId, result.First().EventTypeId);
        }

        [TestMethod]
        public void Filter_WhenPassedQueryable_ReturnsEmptyQueryableWhenNoEventsHaveEquivalentKeyAndValueInData()
        {
            var serviceUnderTest = new EventFilters()
            {
                DataKey = "HelloWorld",
                DataValue = "HelloWorld"
            };
            var expectedEventTypeId = 2;
            var unexpectedEventTypeId = 1;
            var testInput = new List<Event>()
            {
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = unexpectedEventTypeId,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1),
                    Data = "NotMatched"
                },
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = expectedEventTypeId,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1),
                    Data = JsonConvert.SerializeObject(new
                    {
                        IrrelevantProperty = "HelloWorld",
                        HelloWorld = "HelloBob",
                        AnotherIrrelevantProperty = "IrrelevantValue"
                    })
                }
            }.AsQueryable();


            var result = testInput.WithFilter(serviceUnderTest);


            Assert.AreEqual(0, result.Count());
        }

    }
}
