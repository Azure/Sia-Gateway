using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sia.Data.Incidents.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Alias = table.Column<string>(nullable: false),
                    Team = table.Column<string>(nullable: false),
                    Role = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => new { x.Alias, x.Team, x.Role });
                });

            migrationBuilder.CreateTable(
                name: "TicketingSystems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketingSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventFired = table.Column<DateTime>(nullable: false),
                    EventTypeId = table.Column<long>(nullable: false),
                    IncidentId = table.Column<long>(nullable: true),
                    Occurred = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Engagements",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IncidentId = table.Column<long>(nullable: false),
                    ParticipantAlias = table.Column<string>(nullable: true),
                    ParticipantRole = table.Column<string>(nullable: true),
                    ParticipantTeam = table.Column<string>(nullable: true),
                    TimeDisengaged = table.Column<DateTime>(nullable: true),
                    TimeEngaged = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engagements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Engagements_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Engagements_Participants_ParticipantAlias_ParticipantTeam_ParticipantRole",
                        columns: x => new { x.ParticipantAlias, x.ParticipantTeam, x.ParticipantRole },
                        principalTable: "Participants",
                        principalColumns: new[] { "Alias", "Team", "Role" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IncidentId = table.Column<long>(nullable: false),
                    IsPrimary = table.Column<bool>(nullable: false),
                    OriginId = table.Column<string>(nullable: true),
                    TicketingSystemId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketingSystems_TicketingSystemId",
                        column: x => x.TicketingSystemId,
                        principalTable: "TicketingSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Engagements_IncidentId",
                table: "Engagements",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Engagements_ParticipantAlias_ParticipantTeam_ParticipantRole",
                table: "Engagements",
                columns: new[] { "ParticipantAlias", "ParticipantTeam", "ParticipantRole" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_IncidentId",
                table: "Events",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IncidentId",
                table: "Tickets",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketingSystemId",
                table: "Tickets",
                column: "TicketingSystemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Engagements");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropTable(
                name: "TicketingSystems");
        }
    }
}
