using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Noter.API.Models;
using Noter.DAL.User;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Noter.API.Controllers
{
    [Route("api")]
    public class UsersController: Controller
    {
        private readonly UserManager<NoterUser> userManager;

        public UsersController(UserManager<NoterUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (registerModel == null)
            {
                return BadRequest();
            }

            var user = await userManager.FindByNameAsync(registerModel.UserName);

            if (user == null)
            {
                user = new NoterUser
                {
                    UserName = registerModel.UserName
                };

                var result = await userManager.CreateAsync(user, registerModel.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("error", error.Description);

                    return BadRequest(ModelState);
                };

            }
            return NoContent();

        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (loginModel == null)
            {
                return BadRequest();
            }
            
            var user = await userManager.FindByNameAsync(loginModel.UserName);

            if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var identity = new ClaimsIdentity("cookies");
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(identity));

                return Ok();
            }

            ModelState.AddModelError("", "Invalid username or password");

            return BadRequest(ModelState);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("cookies");

            return NoContent();
        }

        [HttpGet("authenticated")]        
        public IActionResult IsAuthenticated()
        {
            var result = HttpContext.User.Identity.IsAuthenticated;

            return Ok(result);
        }

        [HttpGet("username")]
        public IActionResult GetUserName()
        {
            var result = HttpContext.User.Identity.Name;

            return Ok(result);
        }

    }
}
