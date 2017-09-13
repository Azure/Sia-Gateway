using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Sia.Data.Incidents;

namespace Sia.Data.Incidents.Migrations
{
    [DbContext(typeof(IncidentContext))]
    partial class IncidentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sia.Data.Incidents.Models.Engagement", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("IncidentId");

                    b.Property<string>("ParticipantAlias");

                    b.Property<string>("ParticipantRole");

                    b.Property<string>("ParticipantTeam");

                    b.Property<DateTime?>("TimeDisengaged");

                    b.Property<DateTime>("TimeEngaged");

                    b.HasKey("Id");

                    b.HasIndex("IncidentId");

                    b.HasIndex("ParticipantAlias", "ParticipantTeam", "ParticipantRole");

                    b.ToTable("Engagements");
                });

            modelBuilder.Entity("Sia.Data.Incidents.Models.Event", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EventFired");

                    b.Property<long>("EventTypeId");

                    b.Property<long?>("IncidentId");

                    b.Property<DateTime>("Occurred");

                    b.HasKey("Id");

                    b.HasIndex("IncidentId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Sia.Data.Incidents.Models.Incident", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Incidents");
                });

            modelBuilder.Entity("Sia.Data.Incidents.Models.Participant", b =>
                {
                    b.Property<string>("Alias");

                    b.Property<string>("Team");

                    b.Property<string>("Role");

                    b.HasKey("Alias", "Team", "Role");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("Sia.Data.Incidents.Models.Ticket", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("IncidentId");

                    b.Property<bool>("IsPrimary");

                    b.Property<string>("OriginId");

                    b.Property<long>("TicketingSystemId");

                    b.HasKey("Id");

                    b.HasIndex("IncidentId");

                    b.HasIndex("TicketingSystemId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Sia.Data.Incidents.Models.TicketingSystem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("TicketingSystems");
                });

            modelBuilder.Entity("Sia.Data.Incidents.Models.Engagement", b =>
                {
                    b.HasOne("Sia.Data.Incidents.Models.Incident")
                        .WithMany("Engagements")
                        .HasForeignKey("IncidentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sia.Data.Incidents.Models.Participant", "Participant")
                        .WithMany()
                        .HasForeignKey("ParticipantAlias", "ParticipantTeam", "ParticipantRole");
                });

            modelBuilder.Entity("Sia.Data.Incidents.Models.Event", b =>
                {
                    b.HasOne("Sia.Data.Incidents.Models.Incident")
                        .WithMany("Events")
                        .HasForeignKey("IncidentId");
                });

            modelBuilder.Entity("Sia.Data.Incidents.Models.Ticket", b =>
                {
                    b.HasOne("Sia.Data.Incidents.Models.Incident", "Incident")
                        .WithMany("Tickets")
                        .HasForeignKey("IncidentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Sia.Data.Incidents.Models.TicketingSystem", "TicketingSystem")
                        .WithMany()
                        .HasForeignKey("TicketingSystemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
