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

namespace AVP.WebApi.Services
{
    public interface IAuthService
    {
        Task<ClaimsIdentity> Login(ApplicationUser user);

        Task<ApplicationUser> RegisterUser(ApplicationUser user);

        string GetUserNameFromToken(HttpContext context);
    }

    public class AuthService : IAuthService
    {
        public IDAO _db;

        public AuthService(IDAO dao)
        {
            _db = dao;
        }

        public async Task<ApplicationUser> RegisterUser(ApplicationUser user)
        {
            //hash the password
            user.PasswordHash = HashPassword(user.Password);

            //add the new user
            return await _db.AddUser(user);
        }

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
                var ClientSecret = "needtogetthisfromenvironment";

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
