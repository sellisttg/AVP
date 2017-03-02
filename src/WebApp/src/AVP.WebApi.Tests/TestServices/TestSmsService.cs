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
    public class TestSmsService : ISmsService
    {
        public Task<int> SendSmsForNotification(Notification notification, List<UserSmsLocation> locations)
        {
            //don't send any messages, just assume the happy path
            return Task.FromResult(0);
        }
    }
}
