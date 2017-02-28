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

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AVP.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : IBaseController
    {
        private IDAO _dao;
        private readonly ILogger _logger;
        private IAuthService _authService;

        public NotificationController(IDAO dao, IAuthService authService, ILoggerFactory loggerFactory)
        {
            _dao = dao;
            _authService = authService;
        }

        [HttpGet("/api/v1/notification")]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                return new OkObjectResult(await _dao.GetAllNotifications());
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error getting all notifications. The error was: {e.Message}, strack trace was: {e.StackTrace}");
                return BadRequest($"Error getting all notifications. The error was: {e.Message}");
            }
        }

        [HttpGet("/api/v1/notification/{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            try
            {
                return new OkObjectResult(await _dao.GetNotificationById(id));
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error getting notification #{id}. The error was: {e.Message}, strack trace was: {e.StackTrace}");
                return BadRequest($"Error getting all notification #{id}. The error was: {e.Message}");
            }
        }

        [HttpPost("/api/v1/notification/new")]
        public async Task<IActionResult> CreateNotification([FromBody]Notification notification)
        {
            try
            {
                return new OkObjectResult(await _dao.InsertNotification(notification));
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error creating notification. The error was: {e.Message}, strack trace was: {e.StackTrace}");
                return BadRequest($"Error creating notification. The error was: {e.Message}");
            }
        }


        [HttpPost("/api/v1/notification/update")]
        public async Task<IActionResult> UpdateNotification([FromBody]Notification notification)
        {
            try
            {
                return new OkObjectResult(await _dao.UpdateNotification(notification));
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error updating notification. The error was: {e.Message}, strack trace was: {e.StackTrace}");
                return BadRequest($"Error updating notification. The error was: {e.Message}");
            }
        }
    }
}
