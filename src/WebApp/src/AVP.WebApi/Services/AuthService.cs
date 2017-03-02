using AVP.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using AVP.DataAccess;
using Microsoft.AspNetCore.Http;
using Jwt;
using AVP.WebApi.Config;
using Microsoft.Extensions.Options;

namespace AVP.WebApi.Services
{
    /// <summary>
    /// Interface to define AVP.WebApi Authentication Services
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Login method
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <returns>ClaimsIdentity</returns>
        Task<ClaimsIdentity> Login(ApplicationUser user);
        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <returns>ApplicationUser</returns>
        Task<ApplicationUser> RegisterUser(ApplicationUser user);
        /// <summary>
        /// Change password for user
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <returns>ApplicationUser</returns>
        Task<ApplicationUser> ChangePassword(ApplicationUser user);
        /// <summary>
        /// Parses JWT Token and returns the SUB (username) from the token
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>string username</returns>
        string GetUserNameFromToken(HttpContext context);
    }

    /// <summary>
    /// Implementation of IAuthService
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// Instance of IDAO injected via dependency injection
        /// </summary>
        public IDAO _db;
        private JwtIssuerOptions _jwtOptions;

        /// <summary>
        /// Constructor, handles injection of IDAO implementation
        /// </summary>
        /// <param name="dao">DAO</param>
        /// /// <param name="jwtOptions">JWT Config Options</param>
        public AuthService(IDAO dao, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            _db = dao;
        }
        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <returns>ApplicationUser</returns>
        public async Task<ApplicationUser> ChangePassword(ApplicationUser user)
        {
            //hash the password
            user.PasswordHash = HashPassword(user.Password);

            //update the password
            return await _db.UpdateUserPassword(user);
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <returns>ApplicationUser</returns>
        public async Task<ApplicationUser> RegisterUser(ApplicationUser user)
        {
            //hash the password
            user.PasswordHash = HashPassword(user.Password);

            //add the new user
            return await _db.AddUser(user);
        }

        /// <summary>
        /// User login. Returns a claims identity to allow JWT generation.
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <returns>ClaimsIdentity</returns>
        public async Task<ClaimsIdentity> Login(ApplicationUser user)
        {
            ApplicationUser userFromDb = await GetUserByName(user.UserName);

            if (userFromDb == null)
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }
            else if (VerifyHashedPassword(userFromDb.PasswordHash, user.Password))
            { //successful login
                if (userFromDb.Role == "admin") //admin user
                {
                    ClaimsIdentity id = new ClaimsIdentity(
                        new GenericIdentity(user.UserName, "Token"),
                        new[]
                        {
                            new Claim("IsAdmin", "true"),
                            new Claim(ClaimTypes.Role, "Admin")
                        }
                    );

                    user.IsAdmin = true;
                    return await Task.FromResult(id);
                }
                else //regular user
                {
                    return await Task.FromResult(new ClaimsIdentity(
                        new GenericIdentity(user.UserName, "Token"),
                        new Claim[] { }));
                }
            }
            else //login failed
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }
        }

        /// <summary>
        /// Parses JWT token from HTTPContext Authorization Header and returns string of SUB (username) property
        /// </summary>
        /// <param name="context">HTTPContext</param>
        /// <returns>string</returns>
        public string GetUserNameFromToken(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                string authHeader = context.Request.Headers["Authorization"];
                var authBits = authHeader.Split(' ');
                if (authBits.Length != 2)
                {
                    //return "{error:\"auth bits needs to be length 2\"}";
                    throw new Exception("Unable to parse username from token. Header does not include bearer and token.");
                }
                if (!authBits[0].ToLowerInvariant().Equals("bearer"))
                {
                    //return "{error:\"authBits[0] must be bearer\"}";
                    throw new Exception("Unable to parse username from token. Bearer token not available in header.");
                }
                var ClientSecret = _jwtOptions.Secret;

                try
                {
                    var data = JsonWebToken.DecodeToObject<Dictionary<string, string>>(authBits[1], ClientSecret);
                    return data["sub"];

                } catch
                {
                    throw new Exception("Unable to parse username from token, or token is invalid.");
                }               
            }

            throw new Exception("Unable to parse username from token, or token is invalid.");

        }

        /// <summary>
        /// Get ApplicationUser from username
        /// </summary>
        /// <param name="user_name">string</param>
        /// <returns>ApplicationUser</returns>
        private async Task<ApplicationUser> GetUserByName(string user_name)
        {
            return await _db.GetUser(user_name);
        }

        //password hashing etc.
        private string HashPassword(string password)
        {
            var prf = KeyDerivationPrf.HMACSHA256;
            var rng = RandomNumberGenerator.Create();
            const int iterCount = 10000;
            const int saltSize = 128 / 8;
            const int numBytesRequested = 256 / 8;

            // Produce a version 3 (see comment above) text hash.
            var salt = new byte[saltSize];
            rng.GetBytes(salt);
            var subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01; // format marker
            WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
            WriteNetworkByteOrder(outputBytes, 5, iterCount);
            WriteNetworkByteOrder(outputBytes, 9, saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
            return Convert.ToBase64String(outputBytes);
        }

        private bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            // Wrong version
            if (decodedHashedPassword[0] != 0x01)
                return false;

            // Read header information
            var prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedHashedPassword, 1);
            var iterCount = (int)ReadNetworkByteOrder(decodedHashedPassword, 5);
            var saltLength = (int)ReadNetworkByteOrder(decodedHashedPassword, 9);

            // Read the salt: must be >= 128 bits
            if (saltLength < 128 / 8)
            {
                return false;
            }
            var salt = new byte[saltLength];
            Buffer.BlockCopy(decodedHashedPassword, 13, salt, 0, salt.Length);

            // Read the subkey (the rest of the payload): must be >= 128 bits
            var subkeyLength = decodedHashedPassword.Length - 13 - salt.Length;
            if (subkeyLength < 128 / 8)
            {
                return false;
            }
            var expectedSubkey = new byte[subkeyLength];
            Buffer.BlockCopy(decodedHashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

            // Hash the incoming password and verify it
            var actualSubkey = KeyDerivation.Pbkdf2(providedPassword, salt, prf, iterCount, subkeyLength);
            return actualSubkey.SequenceEqual(expectedSubkey);
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)(buffer[offset + 0]) << 24)
                | ((uint)(buffer[offset + 1]) << 16)
                | ((uint)(buffer[offset + 2]) << 8)
                | ((uint)(buffer[offset + 3]));
        }
    }
}
