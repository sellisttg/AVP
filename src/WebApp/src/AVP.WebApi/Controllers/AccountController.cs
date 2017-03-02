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
    /// <summary>
    /// Controller for all Account related actions
    /// </summary>
    [Route("api/[controller]")]
    public class AccountController : IBaseController
    {
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;

        private IAuthService _authService;

        /// <summary>
        /// Constructor handles inection of services and options
        /// </summary>
        /// <param name="jwtOptions">JWT Server configuration options</param>
        /// <param name="loggerFactory">ILoggerFactory</param>
        /// <param name="authService">IAuthService</param>
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

        /// <summary>
        /// Register new user from an application user object.
        /// </summary>
        /// <param name="applicationUser">Application user with username and password.</param>
        /// <returns>JWT Token for user login.</returns>
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
                _logger.LogInformation($"Error creating new user, requested username was {applicationUser.UserName}. Exception was: {e.Message}");
                return BadRequest("Error creating user. Please enter a different username and try again.");
            }
        }

        /// <summary>
        /// User log in.
        /// </summary>
        /// <param name="applicationUser">Requres an Application User formatted object. Only username and password are required.</param>
        /// <returns>JSON formatted JWT token.</returns>
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
        /// <summary>
        /// Change user password. Requires authorization, verifies that the token user matches the user requested
        /// </summary>
        /// <param name="applicationUser">Application User serialized as JSON. Only requires username and password fields.</param>
        /// <returns>Application User object</returns>
        [HttpPost("/api/v1/account/changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ApplicationUser applicationUser)
        {
            var userName = _authService.GetUserNameFromToken(this.HttpContext);

            if (!applicationUser.UserName.Equals(userName))
            {
                return BadRequest("User is not authorized to update this password.");
            }
            ApplicationUser user = await _authService.ChangePassword(applicationUser);
            user.PasswordHash = "";
            user.Password = "";
            return new OkObjectResult(user);
        }

        /// <summary>
        /// Generates a JWT Token for a given user's ClaimsIdentity
        /// </summary>
        /// <param name="user">ClaimsIdentity populated with the user's claims</param>
        /// <returns>JWT Token as string</returns>
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
        /// <summary>
        /// Checks for errors in JWT Configuration and throws errors accordingly.
        /// </summary>
        /// <param name="options">JWT Token parameters.</param>
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