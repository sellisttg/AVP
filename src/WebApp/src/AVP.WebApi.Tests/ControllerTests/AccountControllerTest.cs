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

namespace AVP.WebApi.Tests
{
    public class AccountControllerTest
    {
        private Microsoft.Extensions.Options.IOptions<JwtIssuerOptions> _jwtOptions;
        private Microsoft.Extensions.Logging.ILoggerFactory _logger;
        private JsonSerializerSettings _serializerSettings;
        private IAuthService _authService = new TestAuthService();

        private HttpResponseMessage _response;
        private string _token;
        private const string ServiceBaseURL = "http://localhost:57123/";
        private const string SecretKey = "needtogetthisfromenvironment";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));


        public void Setup()
        {
            JwtIssuerOptions options = new JwtIssuerOptions()
            {
                Issuer = "AVPTokenServer",
                Audience = "http://localhost:57123/",
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            };

            //IOptions<JwtIssuerOptions> jwtOptions = new 
            //Microsoft.Extensions.Options.OptionsManager<JwtIssuerOptions> jwtOptions = new Microsoft.Extensions.Options.OptionsManager<JwtIssuerOptions>(options);
        }

        [Fact]
        public void LoginUserTest()
        {
            var webHostBuilder = new WebHostBuilder()
            .UseStartup<Startup>()
            .ConfigureServices(services =>
            {
                services.Configure<JwtIssuerOptions>(options =>
                {
                    options.Issuer = "AVPTokenServer";
                    options.Audience = "http://localhost:57123/";
                    options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                });
            });

            using (var host = new TestServer(webHostBuilder))
            {
                using (var client = host.CreateClient())
                {

                    //serviceProvider.GetService<ILoggingFactory>();
                    //var AccountController = new AccountController(host.Host.Services.GetService<JwtIssuerOptions>(), _logger, _authService)
                    //{

                    //};
                }
            }

           
        }
    }
}
