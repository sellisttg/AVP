using AVP.WebApi.Config;
using AVP.WebApi.Services;
using AVP.WebApi.Controllers;
using Castle.Core.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AVP.WebApi.Tests
{
    public class AccountControllerTest
    {
        private IAuthService _authService;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;

        private HttpResponseMessage _response;
        private string _token;
        private const string ServiceBaseURL = "http://localhost:57123/";

        [Test]
        public void LoginUserTest()
        {
            var AccountController = new AccountController()
            {

            }
        }
    }
}
