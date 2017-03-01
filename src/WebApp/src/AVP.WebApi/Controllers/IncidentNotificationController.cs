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
    public class IncidentNotificationController : IBaseController
    {
        private IDAO _dao;
        private readonly ILogger _logger;
        private IAuthService _authService;

        public IncidentNotificationController(IDAO dao, IAuthService authService, ILoggerFactory loggerFactory)
        {
            _dao = dao;
            _authService = authService;
        }

        [HttpGet("/api/v1/incident")]
        public async Task<IActionResult> GetAllIncidents()
        {
            try
            {
                IncidentsWrapper wrapper = new IncidentsWrapper()
                {
                    incidents = await _dao.GetAllIncidents()
                };

                await _dao.GetSubscribersForIncidents(wrapper.incidents);

                return new OkObjectResult(wrapper);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error getting all incidents. The error was: {e.Message}, strack trace was: {e.StackTrace}");
                return BadRequest($"Error getting all incidents. The error was: {e.Message}");
            }
        }

        [HttpGet("/api/v1/incident/allsubscribers")]
        public async Task<IActionResult> GetAllSubscribers()
        {
            try
            {
                //return all possible subscribers
                SubscribersWrapper subscribers = new SubscribersWrapper()
                {
                    Subscribers = await _dao.GetAllSubscribers()
                };
                return new OkObjectResult(subscribers);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error getting subscribers. The error was: {e.Message}, strack trace was: {e.StackTrace}");
                return BadRequest($"Error getting subscribers. The error was: {e.Message}");
            }
        }
        
        [HttpPost("/api/v1/incident")]
        public async Task<IActionResult> CreateNotificationIncident([FromBody]IncidentsWrapper wrapper)
        {
            try
            {
                //create the incident
                await _dao.CreateIncidents(wrapper.incidents);
                //return all possible subscribers
                SubscribersWrapper subscribers = new SubscribersWrapper()
                {
                    Subscribers = await _dao.GetAllSubscribers()
                };
                return new OkObjectResult(subscribers);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error creating notification incident. The error was: {e.Message}, strack trace was: {e.StackTrace}");
                return BadRequest($"Error creating notification incident. The error was: {e.Message}");
            }
        }


        [HttpPost("/api/v1/incident/subscribersundernotification")]
        public async Task<IActionResult> SubscribersUnderNotification([FromBody]SubscriberUnderNotificationWrapper wrapper)
        {
            try
            {
                //add subscribers to the incident
                await _dao.AddSubscribersToIncident(wrapper.SubscriberUnderNotification);
                //return all possible subscribers
                return new OkObjectResult("Successfully added subscribers to incident");
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error updating subscribers under notification. The error was: {e.Message}, strack trace was: {e.StackTrace}");
                return BadRequest($"Error updating subscribers under notification. The error was: {e.Message}");
            }
        }

        //[HttpGet("/api/v1/incident/test")]
        //public async Task<IActionResult> GetSubWrapper()
        //{
        //    List<Incident> incidents = await _dao.GetAllIncidents();
        //    SubscriberUnderNotificationWrapper wrapper = new SubscriberUnderNotificationWrapper()
        //    {
        //        incident = incidents.First(),
        //        SubscriberUnderNotification = await _dao.GetAllSubscribers()
        //    };
        //    return new OkObjectResult(wrapper);
        //}

    }
}
