using Ecommorce.API.Helper;
using Ecommorce.Core.DTO;
using Ecommorce.Core.Entities.AppUser;
using Ecommorce.Core.Services;
using Ecommorce_.infrastructure.Repositories.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommorce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account2Controller : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;

        public Account2Controller(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login2")]
        public async Task<IActionResult> Login2([FromBody] LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password." });

            var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!checkPassword.Succeeded)
                return Unauthorized(new { message = "Invalid email or password." });

            var token = _jwtTokenService.GenerateAndSetToken(user);

            return Ok(new ApiResponse(200, "Logged in successfully"));
        }
    }
}
