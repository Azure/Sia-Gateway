using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Data.Incidents.Migrations
{
    internal class MigrationConfigurationLoader : IDesignTimeDbContextFactory<IncidentContext>
    {
        public MigrationConfigurationLoader()
        {
        }

        public IncidentContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddUserSecrets<MigrationConfigurationLoader>()
                .Build();
            var builder = new DbContextOptionsBuilder<IncidentContext>()
                .UseSqlServer(configuration.GetConnectionString("MigrationTarget"));

            return new IncidentContext(builder.Options);
        }
    }
}
