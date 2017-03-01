using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AVP.DataAccess;
using AVP.WebApi.Services;
using AVP.Models.Entities;

namespace AVP.WebApi.Controllers
{
    /// <summary>
    /// Controller for all UserSmsLocation actions
    /// </summary>
    [Route("api/[controller]")]
    public class UserSmsLocationController : IBaseController
    {
        private IDAO _dao;
        private IAuthService _authService;
        /// <summary>
        /// Constructor for UserSMSLocationController.
        /// </summary>
        /// <param name="dao">Data Access Object Service</param>
        /// <param name="authService">User Authentication Service</param>
        public UserSmsLocationController(IDAO dao, IAuthService authService)
        {
            _dao = dao;
            _authService = authService;
        }
        /// <summary>
        /// Get all UserSmsLocations for the logged in user
        /// </summary>
        /// <returns>List of UserSmsLocations</returns>
        [HttpGet("/api/v1/smslocation/")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userName = _authService.GetUserNameFromToken(this.HttpContext);

                return new JsonResult(await _dao.GetUserSmsLocationsForUser(userName));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Get a UserSmsLocation object by id.
        /// </summary>
        /// <param name="id">Id of User Sms Location</param>
        /// <returns>Requested UserSmsLocation object</returns>
        [HttpGet("/api/v1/smslocation/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                return new JsonResult(await _dao.GetUserSmsLocationById(id));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Update UserSmsLocation
        /// </summary>
        /// <param name="smsLoc">UserSmsLocation to update</param>
        /// <returns>Updated UserSmsLocation</returns>
        [HttpPost("/api/v1/smslocation/")]
        public async Task<IActionResult> Post([FromBody]UserSmsLocation smsLoc)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                return new JsonResult(await _dao.UpdateUserSmsLocation(smsLoc));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Add new User SMS Location
        /// </summary>
        /// <param name="smsLoc">UserSmsLocation Object to add</param>
        /// <returns>Inserted UserSmsLocation object with Id</returns>
        [HttpPut("/api/v1/smslocation/")]
        public async Task<IActionResult> Put([FromBody]UserSmsLocation smsLoc)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                return new JsonResult(await _dao.InsertUserSmsLocation(smsLoc));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Delete UserSmsLocationObject
        /// </summary>
        /// <param name="smsLoc">UserSmsLocationObject to delete</param>
        /// <returns>Success or failure string</returns>
        [HttpDelete("/api/v1/smslocation/")]
        public async Task<IActionResult> Delete([FromBody]UserSmsLocation smsLoc)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                if (await _dao.DeleteUserSmsLocation(smsLoc))
                {
                    return new JsonResult("Successfully deleted address.");
                }
                else
                {
                    return BadRequest("Error deleting address");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
