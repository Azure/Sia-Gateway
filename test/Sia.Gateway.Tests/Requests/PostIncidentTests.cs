using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain;
using Sia.Domain.ApiModels;
using Sia.Gateway.Requests;
using Sia.Gateway.ServiceRepositories;
using Sia.Gateway.Tests.TestDoubles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sia.Gateway.Tests.Requests
{
    [TestClass]
    public class PostIncidentTests
    {
        private IMapper _mapper;

        [TestInitialize]
        public void ConfigureAutomapper()
        {
            Mapper.Initialize(configuration =>
            {
                configuration.CreateMap<NewIncident, Incident>();
            });
            _mapper = Mapper.Instance;
        }

        [TestMethod]
        public async Task Handle_WhenIncidentClientReturnsSuccessful_ReturnCorrectIncidents()
        {
            string expectedIncidentTitle = "The thing we were looking for";
            var expectedIncident = new NewIncident
            {
                Title = expectedIncidentTitle,
            };
            IIncidentRepository mockRepository = new StubIncidentRepository(new List<Incident>(), _mapper);
            var serviceUnderTest = new PostIncidentHandler(mockRepository);
            var request = new PostIncidentRequest(expectedIncident, new DummyAuthenticatedUserContext());


            var result = await serviceUnderTest.Handle(request);


            Assert.AreEqual(expectedIncidentTitle, result.Title);
        }
    }
}
