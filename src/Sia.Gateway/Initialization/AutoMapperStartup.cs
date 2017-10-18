using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Protocol;
using Sia.Shared.Data;

namespace Sia.Gateway.Initialization
{
    public static class AutoMapperStartup
    {
        public static void InitializeAutomapper()
        {
            Mapper.Initialize(configuration =>
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

        private static IMappingExpression<TSource, TDestination> UseResolveJsonToString<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
            where TSource: IJsonDataObject
            where TDestination: IJsonDataString
            => mapping.ForMember((ev) => ev.Data, (config) => config.ResolveUsing<ResolveJsonToString<TSource, TDestination>>());


        private static IMappingExpression<TSource, TDestination> UseResolveStringToJson<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
            where TSource : IJsonDataString
            where TDestination : IJsonDataObject
            => mapping.ForMember((ev) => ev.Data, (config) => config.ResolveUsing<ResolveStringToJson<TSource, TDestination>>());


        public static IMappingExpression<T1, T2> EqualityInsertOnly<T1, T2>(this IMappingExpression<T1, T2> mappingExpression)
            where T1 : class
            where T2 : class 
            => mappingExpression.EqualityComparison((one, two) => false);

        public static IMappingExpression<T1, T2> EqualityById<T1, T2>(this IMappingExpression<T1, T2> mappingExpression)
            where T1 : class, IEntity
            where T2 : class, IEntity 
            => mappingExpression.EqualityComparison((one, two) => one.Id == two.Id);
    }
}
