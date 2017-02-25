using AVP.Models.Entities;
using AVP.WebApi.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AVP.WebApi.Tests.TestServices
{
    public class TestAuthService : IAuthService
    {
        public async Task<ClaimsIdentity> Login(ApplicationUser user)
        {
            return await Task.FromResult(new ClaimsIdentity(
                        new GenericIdentity(user.UserName, "Token"),
                        new Claim[] { }));
        }

        public async Task<ApplicationUser> RegisterUser(ApplicationUser user)
        {
            return user;
        }

        public string GetUserNameFromToken(HttpContext context)
        {
            return "sellis";
        }
    }
}
