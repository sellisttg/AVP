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
    public class UserEmailLocationController : IBaseController
    {
        private IDAO _dao;
        private IAuthService _authService;

        public UserEmailLocationController(IDAO dao, IAuthService authService)
        {
            _dao = dao;
            _authService = authService;
        }

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
