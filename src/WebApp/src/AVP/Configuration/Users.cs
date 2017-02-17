using IdentityModel;
using IdentityServer4.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AVP.Configuration
{
    internal class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser> {
            new InMemoryUser {
                Subject = "5BE86359-073C-434B-AD2D-A3932222DABE",
                Username = "sam",
                Password = "password",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Email, "sellis@trinitytg.com"),
                    new Claim(JwtClaimTypes.Role, "admin")
                }
            }
        };
        }
    }
}
