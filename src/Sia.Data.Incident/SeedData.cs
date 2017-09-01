using Sia.Data.Incidents.Models;
using System;
using System.Collections.Generic;

namespace Sia.Data.Incidents
{
    public static class SeedData
    {
        //Some dev/test/demo data that was based on actual incidents has been [REDACTED]
        public static void Add(IncidentContext incidentContext)
        {
            var firstTestIncidentSystem = new TicketingSystem
            {
                Id = 1,
                Name = "Not Our Ticketing System Name"
            };
            var secondTestIncidentSystem = new TicketingSystem
            {
                Id = 2,
                Name = "Some other ticketing system"
            };

            var firstTestCrisis = new Incident()
            {
                PrimaryTicket = new Ticket()
                {
                    OriginId = "38502026",
                    TicketingSystemId = 1
                },
                Tickets = new List<Ticket>()
                {
                    new Ticket()
                    {
                        OriginId = "44444444",
                        TicketingSystemId = 1
                    }
                },
                Engagements = new List<Engagement>
                {
                    new Engagement
                    {
                        TimeEngaged = DateTime.Parse("03/03/1973"),
                        TimeDisengaged = DateTime.Parse("04/04/1974"),
                        Participant = new Participant
                        {
                            Alias = "pdimit",
                            Team = "Sia Engineering",
                            Role = "Crisis Manager"
                        }
                    },
                    new Engagement
                    {
                        TimeEngaged = DateTime.Parse("03/03/1973"),
                        Participant = new Participant
                        {
                            Alias = "satyan",
                            Team = "Senior Leadership",
                            Role = "VIP"
                        }
                    }
                },
                Events = new List<Event>
                {
                    new Event
                    {
                        EventTypeId = 1,
                        Occurred = DateTime.Parse("05/05/1975"),
                        EventFired = DateTime.Parse("06/06/1976")
                    }
                },
                Title = "Customers are unable to access [REDACTED] from [REDACTED]"
            };
            var secondTestCrisis = new Incident()
            {
                Tickets = new List<Ticket>(),
                Title = "Loss of [REDACTED] Connectivity in [REDACTED]",
                Events = new List<Event>(),
                Engagements = new List<Engagement>
                {
                    new Engagement
                    {
                        TimeEngaged = DateTime.Parse("03/03/1973"),
                        TimeDisengaged = DateTime.Parse("04/04/1974"),
                        Participant = new Participant
                        {
                            Alias = "jache",
                            Team = "Sia Engineering",
                            Role = "Crisis Manager"
                        }
                    }
                },
                PrimaryTicket = new Ticket()
                {
                    OriginId = "38805418",
                    TicketingSystemId = 1
                }
            };
            var thirdTestCrisis = new Incident()
            {
                Tickets = new List<Ticket>(),
                Title = "[REDACTED] and [REDACTED] service management operations for a subset of users in [REDACTED] are failing",
                Events = new List<Event>(),
                Engagements = new List<Engagement>
                {
                    new Engagement
                    {
                        TimeEngaged = DateTime.Parse("03/03/1973"),
                        TimeDisengaged = DateTime.Parse("04/04/1974"),
                        Participant = new Participant
                        {
                            Alias = "magpint",
                            Team = "Sia Engineering",
                            Role = "Crisis Manager"
                        }
                    }
                },
                PrimaryTicket = new Ticket()
                {
                    OriginId = "38808134",
                    TicketingSystemId = 1
                }
            };
            var fourthTestCrisis = new Incident()
            {
                Tickets = new List<Ticket>(),
                Title = "Unable to start [REDACTED] in [REDACTED]",
                Events = new List<Event>(),
                Engagements = new List<Engagement>
                {
                    new Engagement
                    {
                        TimeEngaged = DateTime.Parse("03/03/1973"),
                        TimeDisengaged = DateTime.Parse("04/04/1974"),
                        Participant = new Participant
                        {
                            Alias = "mcheck",
                            Team = "Crisis Management Engineering",
                            Role = "Senior Incident Manager"
                        }
                    }
                },
                PrimaryTicket = new Ticket()
                {
                    OriginId = "38805880",
                    TicketingSystemId = 1
                }
            };

            incidentContext.Add(firstTestIncidentSystem);
            incidentContext.Add(secondTestIncidentSystem);

            incidentContext.Add(firstTestCrisis);
            incidentContext.Add(secondTestCrisis);
            incidentContext.Add(thirdTestCrisis);
            incidentContext.Add(fourthTestCrisis);

            incidentContext.SaveChanges();
        }
    }
}
