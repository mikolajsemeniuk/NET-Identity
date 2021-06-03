using System.Security.Claims;
using System.Threading.Tasks;
using app.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<string> AuthMe()
        {
            return Ok("you were authenticated");
        }

        [HttpPost("signin/{email}/{password}")]
        public async Task<ActionResult> SignInUserAsync(string email, string password)
        {
            var result = await _repository.SignInUserAsync(email, password);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(result.Item1), 
                result.Item2);

            return Ok("You were successfully signed in");
        }

        [HttpPost("signup/{username}/{email}/{password}")]
        public async Task<ActionResult> SignUpUserAsync(string username, string email, string password)
        {
            var result = await _repository.SignUpUserAsync(username, email, password);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(result.Item1), 
                result.Item2);

            return Ok("You were successfully signed in");
        }
    }
}