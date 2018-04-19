using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sia.Data.Incidents.Models;
using Sia.Gateway.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Gateway.Tests.Filters
{
    [TestClass]
    public class FilterExtensionsTests
    {
        [TestMethod]
        public void WithFilter_WhenPassedEmptyQueryable_ReturnsEmptyQueryable()
        {
            var serviceUnderTest = new EventFilters();
            var testInput = new List<Event>().AsQueryable();


            var result = testInput.WithFilter(serviceUnderTest);


            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void WithFilter_WhenPassedQueryable_ReturnsOnlyMatchingResultsAsQueryable()
        {
            var serviceUnderTest = new EventFilters()
            {
                IncidentId = 1,
                StartTime = new DateTime(1970, 1, 1),
                EndTime = new DateTime(1980, 3, 2)
            };
            serviceUnderTest.EventTypes.Add(1);
            var testInput = new List<Event>()
            {
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = 1,
                    Occurred = new DateTime(1970, 1, 1),
                    EventFired = new DateTime(1970, 1, 1)
                },
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = 1,
                    Occurred = new DateTime(1975, 1, 1),
                    EventFired = new DateTime(1975, 1, 1),
                    Data = "firstExpectedEvent"
                },
                new Event()
                {
                    IncidentId = 2,
                    EventTypeId = 1,
                    Occurred = new DateTime(1981, 1, 1),
                    EventFired = new DateTime(1981, 1, 1)
                },
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = 2,
                    Occurred = new DateTime(1976, 1, 1),
                    EventFired = new DateTime(1976, 1, 1)
                },
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = 1,
                    Occurred = new DateTime(1986, 2, 2),
                    EventFired = new DateTime(1970, 1, 1)
                },
                new Event()
                {
                    IncidentId = 1,
                    EventTypeId = 1,
                    Occurred = new DateTime(1986, 1, 1),
                    EventFired = new DateTime(1986, 2, 2)
                },
            }.AsQueryable();


            var result = testInput.WithFilter(serviceUnderTest);


            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Select(ev => ev.Data).Contains("firstExpectedEvent"));
        }

        [TestMethod]
        public void WithFilter_WhenPassedQueryable_MatchesByDataKey_WhenEventsHaveEquivalentKeyInData()
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
        public void WithFilter_WhenPassedQueryable_ReturnsEmptyQueryable_WhenNoEventsHaveEquivalentKeyInData()
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
        public void WithFilter_WhenPassedQueryable_MatchesByDataKeyAndValue_WhenEventsHaveEquivalentKeyAndValueInData()
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
        public void WithFilter_WhenPassedQueryable_ReturnsEmptyQueryable_WhenNoEventsHaveEquivalentKeyAndValueInData()
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
