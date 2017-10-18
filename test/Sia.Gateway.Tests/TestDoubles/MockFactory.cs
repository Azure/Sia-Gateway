using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Gateway.Tests.TestDoubles
{
    public static class MockFactory
    {
        /// <summary>
        /// Returns an in-memory Incident Context with seed data
        /// </summary>
        /// <param name="instance">Name of the particular in-memory store to use. Re-use is not suggested when modifying data during test (nameof() the test method is preferred)</param>
        /// <returns></returns>
        public static IncidentContext IncidentContext(string instance)
        {
            var options = new DbContextOptionsBuilder<IncidentContext>()
                                .UseInMemoryDatabase(instance)
                                .Options;
            var context = new IncidentContext(options);
            SeedData.Add(context);

            return context;
        }
    }
}
