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
    public class UserAddressControllerTest
    {
        private TestDAO _dao = new TestDAO();
        private TestAuthService _authService = new TestAuthService();

        [Fact]
        public async Task GetUserAddress()
        {
            //Arrange
            var controller = new UserAddressController(_dao, _authService);

            //Act
            var result = await controller.Get();

            //Assert
            var viewResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task GetUserAddressById()
        {
            //Arrange
            var controller = new UserAddressController(_dao, _authService);

            //Act
            var result = await controller.Get(1);

            //Assert
            var viewResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task UpdateUserAddress()
        {
            UserAddress address = new UserAddress()
            {
                UserAddressID = 1,
                UserID = 1,
                StreetAddress = "123 Any Street",
                City = "Sacramento",
                State = "CA",
                Zip = 95746,
                Latitude = 123.45,
                Longitude = 45.67

            };
            //Arrange
            var controller = new UserAddressController(_dao, _authService);

            //Act failure
            var result = await controller.Post(address);

            //Assert failure
            var failureResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task InsertUserAddress()
        {
            UserAddress address = new UserAddress()
            {
                UserAddressID = 1,
                UserID = 1,
                StreetAddress = "123 Any Street",
                City = "Sacramento",
                State = "CA",
                Zip = 95746,
                Latitude = 123.45,
                Longitude = 45.67

            };
            //Arrange
            var controller = new UserAddressController(_dao, _authService);

            //Act success
            var result = await controller.Put(address);

            //Assert success
            var failureResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task DeleteUserAddress()
        {
            UserAddress address = new UserAddress()
            {
                UserAddressID = 1,
                UserID = 1,
                StreetAddress = "123 Any Street",
                City = "Sacramento",
                State = "CA",
                Zip = 95746,
                Latitude = 123.45,
                Longitude = 45.67

            };
            //Arrange
            var controller = new UserAddressController(_dao, _authService);

            //Act failure
            var result = await controller.Delete(address);

            //Assert failure
            var failureResult = Assert.IsType<JsonResult>(result);
        }
    }
}
