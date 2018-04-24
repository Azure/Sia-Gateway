using Sia.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.State.Services.Demo
{
    public class DemoEventsService
    {
        public IList<Event> Events { get; }
            = new List<Event>()
            {
                new Event()
                {
                    EventTypeId = 1,
                    Data = new DemoBridgeJoinData()
                    {
                        Action = "Joined",
                        Alias = "joej",
                        Service = "Azure Automation",
                        Team = "AzureAutomation-SWE (PG)"
                    }
                },
                new Event()
                {
                    EventTypeId = 1,
                    Data = new DemoBridgeJoinData()
                    {
                        Action = "Joined",
                        Alias = "krishnak",
                        Service = "Azure Automation",
                        Team = "IncidentManager-PG"
                    }
                },
                new Event()
                {
                    EventTypeId = 1,
                    Data = new DemoBridgeJoinData()
                    {
                        Action = "Joined",
                        Alias = "harleyh",
                        Service = "Azure Automation",
                        Team = "AA-IncidentResponse-Lion"
                    }
                },
                new Event()
                {
                    EventTypeId = 1,
                    Data = new DemoBridgeJoinData()
                    {
                        Action = "Joined",
                        Alias = "anirudhg"
                    }
                },
                new Event()
                {
                    EventTypeId = 1,
                    Data = new DemoBridgeJoinData()
                    {
                        Action = "Left",
                        Alias = "joej",
                        Service = "Azure Automation",
                        Team = "AzureAutomation-SWE (PG)"
                    }
                },
                new Event()
                {
                    EventTypeId = 1,
                    Data = new DemoBridgeJoinData()
                    {
                        Action = "Joined",
                        Alias = "v-alcowz",
                        Service = "Azure Communications Manager",
                        Team = "Communication Manager"
                    }
                },
                new Event()
                {
                    EventTypeId = 1,
                    Data = new DemoBridgeJoinData()
                    {
                        Action = "Joined",
                        Alias = "pdimit",
                        Service = "Windows Azure Operations Center",
                        Team = "Incident Manager"
                    }
                }
            };
    }
}
