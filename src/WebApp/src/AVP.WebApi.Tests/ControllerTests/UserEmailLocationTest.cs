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
    public class UserEmailLocationTest
    {
        private TestDAO _dao = new TestDAO();
        private TestAuthService _authService = new TestAuthService();

        [Fact]
        public async Task GetUserEmailLocation()
        {
            //Arrange
            var controller = new UserEmailLocationController(_dao, _authService);

            //Act
            var result = await controller.Get();

            //Assert
            var viewResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task GetUserAddressById()
        {
            //Arrange
            var controller = new UserEmailLocationController(_dao, _authService);

            //Act
            var result = await controller.Get(1);

            //Assert
            var viewResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task UpdateUserAddress()
        {
            UserEmailLocation emailLoc = new UserEmailLocation()
            {
                UserEmailLocationID = 1,
                UserID = 1,
                UserAddressID = 1,
                EmailAddress = "sellis@trinitytg.com"
            };
            //Arrange
            var controller = new UserEmailLocationController(_dao, _authService);

            //Act failure
            var result = await controller.Post(emailLoc);

            //Assert failure
            var failureResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task InsertUserAddress()
        {
            UserEmailLocation emailLoc = new UserEmailLocation()
            {
                UserEmailLocationID = 1,
                UserID = 1,
                UserAddressID = 1,
                EmailAddress = "sellis@trinitytg.com"
            };
            //Arrange
            var controller = new UserEmailLocationController(_dao, _authService);

            //Act success
            var result = await controller.Put(emailLoc);

            //Assert success
            var failureResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task DeleteUserAddress()
        {
            UserEmailLocation emailLoc = new UserEmailLocation()
            {
                UserEmailLocationID = 1,
                UserID = 1,
                UserAddressID = 1,
                EmailAddress = "sellis@trinitytg.com"
            };
            //Arrange
            var controller = new UserEmailLocationController(_dao, _authService);

            //Act failure
            var result = await controller.Delete(emailLoc);

            //Assert failure
            var failureResult = Assert.IsType<JsonResult>(result);
        }
    }
}
