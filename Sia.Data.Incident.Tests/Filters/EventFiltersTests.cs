using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Data.Incidents.Filters;
using Sia.Data.Incidents.Models;
using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Data.Incident.Tests.Filters
{
    [TestClass]
    public class EventFiltersTests
    {
        [TestMethod]
        public void Filter_WhenPassedEmptyQueryable_ReturnsEmptyQueryable()
        {
            var serviceUnderTest = new EventFilters();
            var testInput = new List<Event>().AsQueryable();


            var result = serviceUnderTest.Filter(testInput);


            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void Filter_WhenPassedQueryable_ReturnsOnlyMatchingResultsAsQueryable()
        {
            var serviceUnderTest = new EventFilters()
            {
                IncidentId = 1,
                EventTypeId = 1,
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


            var result = serviceUnderTest.Filter(testInput);


            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result.Select(ev => ev.Data).Contains("firstExpectedEvent"));
            Assert.IsTrue(result.Select(ev => ev.Data).Contains("secondExpectedEvent"));
        }
    }
}
