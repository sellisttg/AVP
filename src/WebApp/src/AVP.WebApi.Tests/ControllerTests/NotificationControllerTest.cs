﻿using AVP.Models.Entities;
using AVP.WebApi.Controllers;
using AVP.WebApi.Tests.TestData;
using AVP.WebApi.Tests.TestServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AVP.WebApi.Tests.ControllerTests
{
    public class NotificationControllerTest
    {
        private TestDAO _dao = new TestDAO();
        private TestAuthService _authService = new TestAuthService();

        [Fact]
        public async Task GetAllNotifications()
        {
            //Arrange
            var controller = new NotificationController(_dao, _authService, new Microsoft.Extensions.Logging.LoggerFactory());

            //Act
            var result = await controller.GetAllNotifications();

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetNotificationById()
        {
            //Arrange
            var controller = new NotificationController(_dao, _authService, new Microsoft.Extensions.Logging.LoggerFactory());

            //Act
            var result = await controller.GetNotificationById(1);

            //Assert
            var viewResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateNotification()
        {
            Notification notification = new Notification()
            {
                NotificationID = 1,
                Message = "Tornado is coming!",
                MessageDateTime = DateTime.Now,
                SendingUserID = 19,
                IncidentID = 1
            };
            //Arrange
            var controller = new NotificationController(_dao, _authService, new Microsoft.Extensions.Logging.LoggerFactory());

            //Act failure
            var result = await controller.UpdateNotification(notification);

            //Assert failure
            var failureResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateNotification()
        {
            Notification notification = new Notification()
            {
                NotificationID = 1,
                Message = "Tornado is coming!",
                MessageDateTime = DateTime.Now,
                SendingUserID = 19,
                IncidentID = 1                
            };
            //Arrange
            var controller = new NotificationController(_dao, _authService, new Microsoft.Extensions.Logging.LoggerFactory());

            //Act success
            var result = await controller.CreateNotification(notification);

            //Assert success
            var failureResult = Assert.IsType<OkObjectResult>(result);
        }
    }
}