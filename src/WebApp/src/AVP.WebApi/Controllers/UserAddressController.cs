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
    [Route("api/[controller]")]
    public class UserAddressController : Controller
    {
        private IDAO _dao;
        private IAuthService _authService;

        public UserAddressController(IDAO dao, IAuthService authService)
        {
            _dao = dao;
            _authService = authService;
        }

        // GET: api/values
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

        // GET api/values/5
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

        // POST api/values
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

        // PUT api/values/5
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

        // DELETE api/values/5
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
