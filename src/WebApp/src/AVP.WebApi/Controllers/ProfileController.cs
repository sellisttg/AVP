using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AVP.DataAccess;
using AVP.Models.Entities;
using Microsoft.AspNetCore.Http;
using AVP.WebApi.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AVP.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ProfileController : IBaseController
    {

        private IDAO _dao;
        private IAuthService _authService; 

        public ProfileController(IDAO dao, IAuthService authService)
        {
            _dao = dao;
            _authService = authService;
        }

        // GET api/values/5
        [HttpGet("/api/v1/profile")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userName = _authService.GetUserNameFromToken(this.HttpContext);

                return new JsonResult(await _dao.GetProfileForUserName(userName));

            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }            
        }

        // POST api/values
        [HttpPost("/api/v1/profile")]
        public async Task<IActionResult> Post([FromBody]UserProfile profile)
        {
            try
            {
                var userName = _authService.GetUserNameFromToken(this.HttpContext);

                if(!profile.UserName.Equals(userName))
                {
                    return BadRequest("User is not authorized to edit this profile.");
                }

                return new JsonResult(await _dao.UpdateUserProfile(profile));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
