using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreList.Application.Interfaces;
using StoreList.Infrastructure.Identity;

namespace StoreList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AddOrUpdateAppUserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (succeeded, token, errors) = await _authService.RegisterAsync(model);

            if (!succeeded)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return BadRequest(ModelState);
            }

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (succeeded, token, errors) = await _authService.LoginAsync(model);

            if (!succeeded)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
                return BadRequest(ModelState);
            }

            return Ok(new { token });
        }
    }
}
