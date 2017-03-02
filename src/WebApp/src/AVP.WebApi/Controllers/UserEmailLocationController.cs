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
    /// Controller for all UserEmailLocation actions
    /// </summary>
    [Route("api/[controller]")]
    public class UserEmailLocationController : IBaseController
    {
        private IDAO _dao;
        private IAuthService _authService;

        /// <summary>
        /// Constructor for UserEmailLocation controller. Loads DAO and AuthServices via dependency injection
        /// </summary>
        /// <param name="dao">DataAccessObject to load via dependency injection</param>
        /// <param name="authService">Auth Service object to load via dependency injection</param>
        public UserEmailLocationController(IDAO dao, IAuthService authService)
        {
            _dao = dao;
            _authService = authService;
        }

        /// <summary>
        /// Get all UserEmailLocations for the currently logged in user
        /// </summary>
        /// <returns>List<UserEmailLocation> emailLocations</UserEmailLocation></returns>
        [HttpGet("/api/v1/emaillocation/")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userName = _authService.GetUserNameFromToken(this.HttpContext);

                return new JsonResult(await _dao.GetUserEmailLocationsForUser(userName));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get UserEmailLocation by ID
        /// </summary>
        /// <param name="id">integer ID of a UserEmailLocation</param>
        /// <returns>UserEmailLocation</returns>
        [HttpGet("/api/v1/emaillocation/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                return new JsonResult(await _dao.GetUserEmailLocationById(id));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Update UserEmailLocation
        /// </summary>
        /// <param name="emailLoc">UserEmailLocation to update</param>
        /// <returns>Updated UserEmailLocation</returns>
        [HttpPost("/api/v1/emaillocation/")]
        public async Task<IActionResult> Post([FromBody]UserEmailLocation emailLoc)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                return new JsonResult(await _dao.UpdateUserEmailLocation(emailLoc));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Insert new UserEmailLocation
        /// </summary>
        /// <param name="emailLoc">UserEmailLocation to insert</param>
        /// <returns>Inserted UserEmailLocation</returns>
        [HttpPut("/api/v1/emaillocation/")]
        public async Task<IActionResult> Put([FromBody]UserEmailLocation emailLoc)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                return new JsonResult(await _dao.InsertUserEmailLocation(emailLoc));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// Delete UserEmailLocation
        /// </summary>
        /// <param name="emailLoc">UserEmailLocation</param>
        /// <returns>Success or failure string. Failure is a 400 response.</returns>
        [HttpDelete("/api/v1/emaillocation/")]
        public async Task<IActionResult> Delete([FromBody]UserEmailLocation emailLoc)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                if (await _dao.DeleteUserEmailLocation(emailLoc))
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
