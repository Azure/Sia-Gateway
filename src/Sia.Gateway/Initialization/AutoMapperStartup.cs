using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Sia.Domain;
using Sia.Domain.ApiModels;

namespace Sia.Gateway.Initialization
{
    public static class AutoMapperStartup
    {
        public static void InitializeAutomapper() 
        => Mapper.Initialize(configuration =>
        {
            configuration.AddCollectionMappers();

            configuration.CreateMap<NewIncident, Data.Incidents.Models.Incident>().EqualityInsertOnly();
            configuration.CreateMap<UpdateIncident, Data.Incidents.Models.Incident>();
            configuration.CreateMap<Incident, Data.Incidents.Models.Incident>().EqualityById();
            configuration.CreateMap<Data.Incidents.Models.Incident, Incident>().EqualityById();

            configuration.CreateMap<Ticket, Data.Incidents.Models.Ticket>().EqualityById();
            configuration.CreateMap<Data.Incidents.Models.Ticket, Ticket>().EqualityById();

            configuration.CreateMap<NewEngagement, Data.Incidents.Models.Engagement>().EqualityInsertOnly();
            configuration.CreateMap<UpdateEngagement, Data.Incidents.Models.Engagement>();
            configuration.CreateMap<Engagement, Data.Incidents.Models.Engagement>().EqualityById();
            configuration.CreateMap<Data.Incidents.Models.Engagement, Engagement>().EqualityById();

            configuration.CreateMap<TicketingSystem, Data.Incidents.Models.TicketingSystem>().EqualityById();
            configuration.CreateMap<Data.Incidents.Models.TicketingSystem, TicketingSystem>().EqualityById();

            configuration.CreateMap<Participant, Data.Incidents.Models.Participant>();
            configuration.CreateMap<Data.Incidents.Models.Participant, Participant>();
            configuration.CreateMap<NewEvent, Data.Incidents.Models.Event>().EqualityInsertOnly()
                .UseResolveJsonToString();
            configuration.CreateMap<Event, Data.Incidents.Models.Event>().EqualityById()
                .UseResolveJsonToString();
            configuration.CreateMap<Data.Incidents.Models.Event, Event>().EqualityById()
                .UseResolveStringToJson();
        });

    }
}
