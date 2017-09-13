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
                Name = "Not Our Ticketing System Name"
            };
            var secondTestIncidentSystem = new TicketingSystem
            {
                Name = "Some other ticketing system"
            };

            var firstTestTicket = new Ticket()
            {
                OriginId = "38502026",
                TicketingSystem = firstTestIncidentSystem,
                IsPrimary = true
            };
            var secondTestTicket = new Ticket()
            {
                OriginId = "44444444",
                TicketingSystem = firstTestIncidentSystem
            };
            var thirdTestTicket = new Ticket()
            {
                OriginId = "38805418",
                TicketingSystem = firstTestIncidentSystem,
                IsPrimary = true
            };

            var firstTestCrisis = new Incident()
            {
                Tickets = new List<Ticket>()
                {
                    firstTestTicket,
                    secondTestTicket
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
                Tickets = new List<Ticket>()
                {
                    thirdTestTicket
                },
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
                }
            };
            var thirdTestCrisis = new Incident()
            {
                Tickets = new List<Ticket>()
                {
                    new Ticket()
                    {
                        OriginId = "38808134",
                        TicketingSystem = firstTestIncidentSystem,
                        IsPrimary = true
                    }
                },
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
                }
            };
            var fourthTestCrisis = new Incident()
            {
                Tickets = new List<Ticket>()
                {
                    new Ticket()
                    {
                        OriginId = "38805880",
                        TicketingSystem = firstTestIncidentSystem,
                        IsPrimary = true
                    }
                },
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
