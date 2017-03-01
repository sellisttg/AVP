using AVP.WebApi.Config;
using AVP.WebApi.Services;
using AVP.WebApi.Controllers;
using Castle.Core.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Xunit;
using AVP.WebApi.Tests.TestServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using AVP.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AVP.WebApi.Tests
{
    public class AccountControllerTest
    {
        private IAuthService _authService = new TestAuthService();


        private const string ServiceBaseURL = "http://localhost:57123/";
        private const string SecretKey = "needtogetthisfromenvironment";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        [Fact]
        public async void LoginUserTest()
        {
            JwtIssuerOptions options = new JwtIssuerOptions()
            {
                Issuer = "AVPTokenServer",
                Audience = "http://localhost:57123/",
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            };
            IOptions<JwtIssuerOptions> jwtOptions = Options.Create<JwtIssuerOptions>(options);
            var AccountController = new AccountController(jwtOptions, new Microsoft.Extensions.Logging.LoggerFactory(), _authService);

            ApplicationUser user = new ApplicationUser()
            {
                UserName = "sellis",
                Password = "password"
            };

            var result =  await AccountController.Post(user);

            var viewResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void ChangeUserPasswordTest()
        {
            JwtIssuerOptions options = new JwtIssuerOptions()
            {
                Issuer = "AVPTokenServer",
                Audience = "http://localhost:57123/",
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            };
            IOptions<JwtIssuerOptions> jwtOptions = Options.Create<JwtIssuerOptions>(options);
            var AccountController = new AccountController(jwtOptions, new Microsoft.Extensions.Logging.LoggerFactory(), _authService);

            ApplicationUser user = new ApplicationUser()
            {
                UserName = "sellis",
                Password = "password"
            };

            var result = await AccountController.Post(user);

            var viewResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void RegisterUserTest()
        {
            JwtIssuerOptions options = new JwtIssuerOptions()
            {
                Issuer = "AVPTokenServer",
                Audience = "http://localhost:57123/",
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            };
            IOptions<JwtIssuerOptions> jwtOptions = Options.Create<JwtIssuerOptions>(options);
            var AccountController = new AccountController(jwtOptions, new Microsoft.Extensions.Logging.LoggerFactory(), _authService);

            ApplicationUser user = new ApplicationUser()
            {
                UserName = "sellis",
                Password = "password"
            };

            var result = await AccountController.Register(user);

            var viewResult = Assert.IsType<OkObjectResult>(result);
        }
    }
}
