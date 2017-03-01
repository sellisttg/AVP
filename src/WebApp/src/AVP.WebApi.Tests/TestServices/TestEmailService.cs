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
    public class TestEmailService : IEmailService
    {
        public async Task<int> SendEmailForNotification(Notification notification, List<UserEmailLocation> locations)
        {
            //don't send any messages, just assume the happy path
            return 0;
        }
    }
}
