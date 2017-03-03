using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AVP.Models.Entities;
using AVP.DataAccess;
using AVP.WebApi.Services;
using AVP.WebApi.Wrappers;
using Microsoft.Extensions.Logging;

namespace AVP.WebApi.Controllers
{
    /// <summary>
    /// Controller for all Notification actions
    /// </summary>
    [Route("api/[controller]")]
    public class DashboardController : IBaseController
    {
        private IDAO _dao;
        private ISmsService _sms;
        private IEmailService _email;
        private readonly ILogger _logger;
        private IAuthService _authService;

        /// <summary>
        /// Controller for all Notification actions
        /// </summary>
        /// <param name="dao">IDAO</param>
        /// <param name="authService">IAuthService</param>
        /// <param name="loggerFactory">ILoggerFactory</param>
        /// <param name="sms">ISmsService</param>
        /// <param name="email">IEmailService</param>
        public DashboardController(IDAO dao, IAuthService authService, ILoggerFactory loggerFactory, ISmsService sms, IEmailService email)
        {
            _sms = sms;
            _email = email;
            _dao = dao;
            _authService = authService;
            _logger = loggerFactory.CreateLogger<DAO>();
        }

        /// <summary>
        /// Get All Notifications
        /// </summary>
        /// <returns>List of Notification objects</returns>
        [AllowAnonymous]
        [HttpPost("/api/v1/dashboard")]
        public async Task<IActionResult> GetAllDashboardNotifications([FromBody] DashboardCriteria criteria)
        {
            try
            {
                return new OkObjectResult(await _dao.GetDashboardNotifications(criteria));
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error getting all dashboard notifications. The error was: {e.Message}, stack trace was: {e.StackTrace}");
                return BadRequest($"Error getting all dashboard notifications. The error was: {e.Message}");
            }
        }
    }
}
