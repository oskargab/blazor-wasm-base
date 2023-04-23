using BlazorApp1.Shared;
using DbRepository;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using BlazorApp1.Server.ModelBinders;

namespace BlazorApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<AuthController> _logger;
        private readonly MyDbContext dbContext;

        public AuthController(ILogger<AuthController> logger, MyDbContext db)
        {
            _logger = logger;
            dbContext = db;

        }

        [HttpGet]
        [Route("user-profile")]
        public async Task<IActionResult> UserProfileAsync([ModelBinder(typeof(UserIdModelBinder))]string userId/*[ModelBinder(typeof(UserIdModelBinder))]string userId*/)
        {
            var user = dbContext.Users.Find(userId);
            return Ok(new UserProfileDto() { Email="rerere", FirstName="rerae", LastName="reare"}  /*userProfile*/);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("google-login")]
        public IActionResult GoogleLogin()
        {
            return Challenge(
                new AuthenticationProperties
                {
                    
                    RedirectUri = Request.Scheme +"://"+ HttpContext.Request.Host.ToString() + "?externalAuth=true"
                },
                GoogleDefaults.AuthenticationScheme
            ); ;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("discord-login")]
        public IActionResult DiscordLogin()
        {

            return Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = Request.Scheme + "://" + HttpContext.Request.Host.ToString() + "?externalAuth=true"

                },
                authenticationSchemes: new string[] { "discord" }
            );
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return Ok("success");
        }
    }
}