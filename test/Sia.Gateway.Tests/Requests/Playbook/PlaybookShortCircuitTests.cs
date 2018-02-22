﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Domain.Playbook;
using Sia.Gateway.Requests;
using Sia.Gateway.Tests.TestDoubles;
using Sia.Shared.Authentication;
using Sia.Shared.Extensions.Mediatr;
using Sia.Gateway.Initialization.Configuration;

namespace Sia.Gateway.Tests.Requests.Playbook
{
    [TestClass]
    public class PlaybookShortCircuitTests
    {

        [TestMethod]
        public void ShouldRequestContinue__Returns_True_If_Playbook_In_Config()
        {
            var mockConfig = new MicroservicesConfig()
            {
                Playbook = "Valid endpoint"
            };
            var mockShortCircuitImplementation = new GetEventTypeShortCircuit(mockConfig);

            var result = mockShortCircuitImplementation.ShouldRequestContinue(mockConfig);

            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void ShouldRequestContinue__Returns_False_If_Playbook_Not_In_Config()
        {
            var mockConfig = new MicroservicesConfig();
            var mockShortCircuitImplementation = new GetGlobalActionsShortCircuit(mockConfig);

            var result = mockShortCircuitImplementation.ShouldRequestContinue(mockConfig);

            Assert.AreEqual(result, false);
        }

    }
}
 