using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace demoApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public dynamic Get()
        {
            return new
            {
                SecureToken = Guid.NewGuid().ToString(),   
                Claims = User.Claims
                .Select(c =>
                new { claimType = c.Type, claimValue = c.Value })
                .ToList()
            };
        }

        [HttpGet("token")]
        public dynamic GetHttp()
        {
            return new
            {
                token = Guid.NewGuid().ToString(),
                Expires = DateTime.Now.AddDays(1),
                Issuer = Environment.MachineName
            };
        }
    }
}
