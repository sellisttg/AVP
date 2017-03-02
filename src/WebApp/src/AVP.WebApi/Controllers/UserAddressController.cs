using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AVP.Models.Entities;
using AVP.WebApi.Services;
using AVP.DataAccess;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AVP.WebApi.Controllers
{
    /// <summary>
    /// Controller for all UserAddress actions
    /// </summary>
    [Route("api/[controller]")]
    public class UserAddressController : Controller
    {
        private IDAO _dao;
        private IAuthService _authService;

        /// <summary>
        /// Constructor for UserAddressController. Configures DAO and Auth Services via dependency injection
        /// </summary>
        /// <param name="dao">IDAO implementation</param>
        /// <param name="authService">IAuthService implementation</param>
        public UserAddressController(IDAO dao, IAuthService authService)
        {
            _dao = dao;
            _authService = authService;
        }

        /// <summary>
        /// Get all UserAddress objects for the logged in user
        /// </summary>
        /// <returns>List<UserAddress></UserAddress></returns>
        [HttpGet("/api/v1/useraddress")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userName = _authService.GetUserNameFromToken(this.HttpContext);

                return new JsonResult(await _dao.GetAddressesForUser(userName));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get UserAddress by ID
        /// </summary>
        /// <param name="id">int ID</param>
        /// <returns>UserAddress</returns>
        [HttpGet("/api/v1/useraddress/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                return new JsonResult(await _dao.GetAddressById(id));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Update UserAddress
        /// </summary>
        /// <param name="address">UserAddress</param>
        /// <returns>UserAddress</returns>
        [HttpPost("/api/v1/useraddress")]
        public async Task<IActionResult> Post([FromBody]UserAddress address)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                return new JsonResult(await _dao.UpdateUserAddress(address));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// New User Address object
        /// </summary>
        /// <param name="address">UserAddress address</param>
        /// <returns>UserAddress address</returns>
        [HttpPut("/api/v1/useraddress")]
        public async Task<IActionResult> Put([FromBody]UserAddress address)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                return new JsonResult(await _dao.InsertUserAddress(address));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

       /// <summary>
       /// Delete UserAddress
       /// </summary>
       /// <param name="address">UserAddress</param>
       /// <returns>Success/fail string. Failures are 400 responses.</returns>
        [HttpDelete("/api/v1/useraddress")]
        public async Task<IActionResult> Delete([FromBody]UserAddress address)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                if(await _dao.DeleteUserAddress(address))
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
