//using AVP.WebApi.Config;
//using AVP.WebApi.Services;
//using AVP.WebApi.Controllers;
//using Castle.Core.Logging;
//using Newtonsoft.Json;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;

//namespace AVP.WebApi.Tests
//{
//    public class AccountControllerTest
//    {
//        private IAuthService _authService;
//        private Microsoft.Extensions.Options.IOptions<JwtIssuerOptions> _jwtOptions;
//        private Microsoft.Extensions.Logging.ILoggerFactory _logger;
//        private JsonSerializerSettings _serializerSettings;

//        private HttpResponseMessage _response;
//        private string _token;
//        private const string ServiceBaseURL = "http://localhost:57123/";
//        private const string SecretKey = "needtogetthisfromenvironment";
//        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

//        [TestFixtureSetUp]
//        public void Setup()
//        {
//            JwtIssuerOptions options = new JwtIssuerOptions()
//            {
//                Issuer = "AVPTokenServer",
//                Audience = "http://localhost:57123/",
//                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
//            };
//            //IOptions<JwtIssuerOptions> jwtOptions = new 
//            //Microsoft.Extensions.Options.OptionsManager<JwtIssuerOptions> jwtOptions = new Microsoft.Extensions.Options.OptionsManager<JwtIssuerOptions>(options);



//        }

//        [Test]
//        public void LoginUserTest()
//        {
//            var AccountController = new AccountController(_jwtOptions, _logger, _authService)
//            {

//            };
//        }
//    }
//}
