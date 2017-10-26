using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.TestDoubles
{
    public static class MockFactory
    {
        /// <summary>
        /// Returns an in-memory Incident Context with seed data
        /// </summary>
        /// <param name="instance">Name of the particular in-memory store to use. Re-use is not suggested when modifying data during test (nameof() the test method is preferred)</param>
        /// <returns></returns>
        public static async Task<IncidentContext> IncidentContext(string instance)
        {
            if (_contexts.TryGetValue(instance, out var context)) return context;
            while(_contextBeingGenerated.TryGetValue(instance, out var beingGenerated)
                && beingGenerated)
            {
                Thread.Sleep(100);
            }

            if(_contextBeingGenerated.TryAdd(instance, true))
            {
                var options = new DbContextOptionsBuilder<IncidentContext>()
                    .UseInMemoryDatabase(instance)
                    .Options;
                context = new IncidentContext(options);
                SeedData.Add(context);
                _contextBeingGenerated.TryAdd(instance, false);
                if (_contexts.TryAdd(instance, context)) return context;
                if (_contexts.TryGetValue(instance, out var otherContext)) return otherContext;
            }

            return context;
        }

        private static ConcurrentDictionary<string, bool> _contextBeingGenerated { get; set; } = new ConcurrentDictionary<string, bool>();
        private static ConcurrentDictionary<string, IncidentContext> _contexts { get; set; } = new ConcurrentDictionary<string, IncidentContext>();
    }
}
