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

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AVP.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : IBaseController
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;

        public IAuthService _authService;


        public AccountController(IOptions<JwtIssuerOptions> jwtOptions, ILoggerFactory loggerFactory, IAuthService authService)
        {
            _authService = authService;

            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);

            _logger = loggerFactory.CreateLogger<AccountController>();

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/api/v1/sessions/register")]
        public async Task<IActionResult> Register([FromBody] ApplicationUser applicationUser)
        {
            //register the user
            try
            {
                if (string.IsNullOrEmpty(applicationUser.UserName))
                    throw new Exception("Please enter a username.");

                if (string.IsNullOrEmpty(applicationUser.Password))
                    throw new Exception("Please enter a password.");

                await _authService.RegisterUser(applicationUser);            

                //log in the newly registered user and return a token to make sure the user credentials are valid
                var identity = await _authService.Login(applicationUser);

                if (identity == null)
                {
                    _logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");
                    return BadRequest("Invalid credentials");
                } else
                {
                    var jwt = await GetJWTForUser(identity);
                    return new OkObjectResult(jwt);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error creating new user, requested username was {applicationUser.UserName}");
                return BadRequest("Error creating user. Please enter a different username and try again.");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/api/v1/sessions")]
        public async Task<IActionResult> Post([FromBody] ApplicationUser applicationUser)
        {
            var identity = await _authService.Login(applicationUser);
            if (identity == null)
            {
                _logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");
                return BadRequest("Invalid credentials");
            }

            var jwt = await GetJWTForUser(identity);
            return new OkObjectResult(jwt);
        }

        private async Task<string> GetJWTForUser(ClaimsIdentity user)
        {
            //return a JWT
            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat,
                          ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(),
                          ClaimValueTypes.Integer64)
            };

            //refresh the options issuedat so that the expiration etc. update
            _jwtOptions.IssuedAt = DateTime.UtcNow;

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Serialize and return the response
            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
            };

            return JsonConvert.SerializeObject(response, _serializerSettings);
            //return new OkObjectResult(json);
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);
    }
}