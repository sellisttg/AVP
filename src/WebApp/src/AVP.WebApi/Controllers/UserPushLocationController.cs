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
    [Route("api/[controller]")]
    public class UserPushLocationController : IBaseController
    {
        private IDAO _dao;
        private IAuthService _authService;

        public UserPushLocationController(IDAO dao, IAuthService authService)
        {
            _dao = dao;
            _authService = authService;
        }

        [HttpGet("/api/v1/pushlocation/")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userName = _authService.GetUserNameFromToken(this.HttpContext);

                return new JsonResult(await _dao.GetUserPushLocationsForUser(userName));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("/api/v1/pushlocation/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                return new JsonResult(await _dao.GetUserPushLocationById(id));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("/api/v1/pushlocation/")]
        public async Task<IActionResult> Post([FromBody]UserPushLocation pushLoc)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                return new JsonResult(await _dao.UpdateUserPushLocation(pushLoc));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("/api/v1/pushlocation/")]
        public async Task<IActionResult> Put([FromBody]UserPushLocation pushLoc)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                return new JsonResult(await _dao.InsertUserPushLocation(pushLoc));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete("/api/v1/pushlocation/")]
        public async Task<IActionResult> Delete([FromBody]UserPushLocation pushLoc)
        {
            try
            {
                string userName = _authService.GetUserNameFromToken(this.HttpContext);

                //todo: get by id and username to verify security
                if (await _dao.DeleteUserPushLocation(pushLoc))
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
