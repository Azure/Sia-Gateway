using Microsoft.EntityFrameworkCore;
using Sia.Data.Incidents.Models;

namespace Sia.Data.Incidents
{
    public class IncidentContext : DbContext
    {
        public IncidentContext(DbContextOptions<IncidentContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Participant>()
                .HasKey(participant => new { participant.Alias, participant.Team, participant.Role });
        }

        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketingSystem> TicketingSystems { get; set; }
        public DbSet<Engagement> Engagements { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}
