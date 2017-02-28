using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Linq;
using AVP.WebApi.Config;
using AVP.Models.Entities;
using AVP.WebApi.Services;
using Xunit;
using AVP.WebApi.Tests.TestData;
using AVP.WebApi.Tests.TestServices;
using AVP.WebApi.Controllers;
using AVP.WebApi.Wrappers;

namespace AVP.WebApi.Tests.ControllerTests
{
    public class IncidentNotificationControllerTest
    {
        private TestDAO _dao = new TestDAO();
        private TestAuthService _authService = new TestAuthService();
        [Fact]
        public async Task GetAllIncidents()
        {
            //Arrange
            var controller = new IncidentNotificationController(_dao, _authService, new Microsoft.Extensions.Logging.LoggerFactory());

            //Act
            var result = await controller.GetAllIncidents();

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateNotificationIncident()
        {
            //Arrange
            var controller = new IncidentNotificationController(_dao, _authService, new Microsoft.Extensions.Logging.LoggerFactory());

            IncidentsWrapper wrapper = new IncidentsWrapper();
            List<Incident> incidents = new List<Incident>();
            wrapper.incidents = incidents;

            //Act
            var result = await controller.CreateNotificationIncident(wrapper);

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task SubscribersUnderNotification()
        {
            //Arrange
            var controller = new IncidentNotificationController(_dao, _authService, new Microsoft.Extensions.Logging.LoggerFactory());

            SubscriberUnderNotificationWrapper wrapper = new SubscriberUnderNotificationWrapper();
            wrapper.incident = new Incident()
            {
                Id = "Tsu470731",
                Lat = 37.788,
                Long = -119.718,
                IncidentType = "Tsunami",
                Radius = 30
            };
            wrapper.SubscriberUnderNotification = await _dao.GetAllSubscribers();

            //Act
            var result = await controller.SubscribersUnderNotification(wrapper);

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllSubscribers()
        {
            //Arrange
            var controller = new IncidentNotificationController(_dao, _authService, new Microsoft.Extensions.Logging.LoggerFactory());

            //Act
            var result = await controller.GetAllSubscribers();

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
        }
    }
}
