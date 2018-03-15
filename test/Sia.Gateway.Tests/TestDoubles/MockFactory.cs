using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Sia.Gateway.Tests.TestDoubles
{
    public static class MockFactory
    {
        /// <summary>
        /// Returns an in-memory Incident Context with seed data
        /// </summary>
        /// <param name="instance">Name of the particular in-memory store to use. Re-use is not suggested when modifying data during test (nameof() the test method is preferred)</param>
        /// <returns></returns>
        public static Task<IncidentContext> IncidentContext(string instance)
        {
            if (_contexts.TryGetValue(instance, out var context)) return Task.FromResult(context);
            while(_contextBeingGenerated.TryGetValue(instance, out var beingGenerated)
                && beingGenerated)
            {
                Thread.Sleep(100);
            }

            if(_contextBeingGenerated.TryAdd(instance, true))
            {
                var options = CreateFreshContextAndDb(instance);
                context = new IncidentContext(options);
                SeedData.Add(context);
                _contextBeingGenerated.TryAdd(instance, false);
                if (_contexts.TryAdd(instance, context)) return Task.FromResult(context);
                if (_contexts.TryGetValue(instance, out var otherContext)) return Task.FromResult(otherContext);
            }

            return Task.FromResult(context);
        }

        private static ConcurrentDictionary<string, bool> _contextBeingGenerated { get; set; } = new ConcurrentDictionary<string, bool>();
        private static ConcurrentDictionary<string, IncidentContext> _contexts { get; set; } = new ConcurrentDictionary<string, IncidentContext>();

        private static DbContextOptions<IncidentContext> CreateFreshContextAndDb(string instance)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<IncidentContext>()
                .UseInMemoryDatabase(instance)
                .UseInternalServiceProvider(serviceProvider);
            return builder.Options;
        }
    }

}
