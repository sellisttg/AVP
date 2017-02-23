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
        [HttpGet("/api/v1/profile/{id}")]
        public async Task<UserProfile> Get(int id)
        {
            HttpContext context = this.HttpContext;
            if(context.Request.Headers.ContainsKey("Authorization"))
            {
                var authHeader = context.Request.Headers["Authorization"];
            }

            return await _dao.GetProfileForUserID(id);
        }

        // POST api/values
        [HttpPost("/api/v1/profile")]
        public void Post([FromBody]UserProfile profile)
        {
        }
    }
}
