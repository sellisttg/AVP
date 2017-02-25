using AVP.Models.Entities;
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
    public class ControllerTest
    {
        private TestDAO _dao = new TestDAO();
        private TestAuthService _authService = new TestAuthService();

        [Fact]
        public async Task GetProfile()
        {
            //Arrange
            var controller = new ProfileController(_dao, _authService);

            //Act
            var result = await controller.Get();

            //Assert
            var viewResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task UpdateProfile()
        {
            UserProfile profile = new UserProfile()
            {
                UserID = 1,
                UserName = "Big Tester",
                SmsOptIn = true,
                EmailOptIn = true,
                PushOptIn = true,
            };
            //Arrange
            var controller = new ProfileController(_dao, _authService);
            
            //Act failure
            var result = await controller.Post(profile);

            //Assert failure
            var failureResult = Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
