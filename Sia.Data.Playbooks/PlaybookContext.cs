using Microsoft.EntityFrameworkCore;
using Sia.Data.Playbooks.Models;


namespace Sia.Data.Playbooks
{
    public class PlaybookContext : DbContext
    {
        public PlaybookContext(DbContextOptions<PlaybookContext> options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.WithManyToManyAssociation<EventType, Action, EventTypeToActionAssociation>
            (
                (eventType) => eventType.ActionAssociations,
                (action) => action.EventTypeAssociations
            );
        }

        public DbSet<Action> Actions { get; set; }
        public DbSet<ActionTemplate> ActionTemplates { get; set; }
        public DbSet<ActionTemplateSource> ActionTemplateSources { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<ConditionSet> ConditionSets { get; set; }
        public DbSet<ConditionSource> ConditionSources { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<EventTypeToActionAssociation> EventTypeToActionAssociations { get; set; }
    }
}
