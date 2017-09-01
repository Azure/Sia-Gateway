using AutoMapper;
using Sia.Domain;
using Sia.Domain.ApiModels;

namespace Sia.Gateway.Initialization
{
    public static class AutoMapperStartup
    {
        public static void InitializeAutomapper()
        {
            Mapper.Initialize(configuration =>
            {
                configuration.CreateMap<NewIncident, Data.Incidents.Models.Incident>();
                configuration.CreateMap<UpdateIncident, Data.Incidents.Models.Incident>();
                configuration.CreateMap<Incident, Data.Incidents.Models.Incident>();
                configuration.CreateMap<Data.Incidents.Models.Incident, Incident>();

                configuration.CreateMap<Ticket, Data.Incidents.Models.Ticket>();
                configuration.CreateMap<Data.Incidents.Models.Ticket, Ticket>();

                configuration.CreateMap<NewEngagement, Data.Incidents.Models.Engagement>();
                configuration.CreateMap<UpdateEngagement, Data.Incidents.Models.Engagement>();
                configuration.CreateMap<Engagement, Data.Incidents.Models.Engagement>();
                configuration.CreateMap<Data.Incidents.Models.Engagement, Engagement>();

                configuration.CreateMap<TicketingSystem, Data.Incidents.Models.TicketingSystem>();
                configuration.CreateMap<Data.Incidents.Models.TicketingSystem, TicketingSystem>();

                configuration.CreateMap<Participant, Data.Incidents.Models.Participant>();
                configuration.CreateMap<Data.Incidents.Models.Participant, Participant>();

                configuration.CreateMap<NewEvent, Data.Incidents.Models.Event>();
                configuration.CreateMap<Event, Data.Incidents.Models.Event>();
                configuration.CreateMap<Data.Incidents.Models.Event, Event>();
            });
        }
    }
}
