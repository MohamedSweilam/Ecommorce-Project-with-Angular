using AutoMapper;
using Ecommorce.API.Helper;
using Ecommorce.Core.DTO;
using Ecommorce.Core.Entities.AppUser;
using Ecommorce.Core.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommorce.API.Controllers
{

    public class AccountController : BaseController
    {
        public AccountController(IUnitofwork unitwork, IMapper mapper) : base(unitwork, mapper)
        {
        }
        [HttpPut("update-address")]
        public async Task<IActionResult> UpdateAddress(shipAddressDTO addressDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var address = _mapper.Map<Address>(addressDTO);
            var result = await _unitwork.Auth.UpdateAddress(email, address);
            return result ? Ok() : BadRequest();
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var result = await _unitwork.Auth.RegisterAsync(registerDTO);
            if(result != "Done")
            {
                return BadRequest(new ApiResponse(400, result));
            }
            return Ok(new ApiResponse(200, result));
        }
        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginDTO login)
        //{
        //    var result = await _unitwork.Auth.LoginAsync(login);

        //    if (result.StartsWith("Please"))
        //        return BadRequest(new ApiResponse(400, result));

        //    // رجّع التوكن في الـ response
        //    return Ok(new
        //    {
        //        token = result
        //    });
        //}


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {

            var result = await _unitwork.Auth.LoginAsync(login);
            if (result.StartsWith("Please"))
                return BadRequest(new ApiResponse(400, result));

            Response.Cookies.Append("token", result, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(1),
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            });

            return Ok(new ApiResponse(200));
        }

        [HttpPost("active-account")]
        public async Task<IActionResult> ActiveAccount(ActiveAccountDTO accountDTO)
        {
            var result = await _unitwork.Auth.ActiveAccount(accountDTO);

            return result ? Ok(new ApiResponse(200)) : BadRequest(new ApiResponse(400));
        }
        [HttpGet("send-email-forget-password")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var result = await _unitwork.Auth.SendEmailToResetPassword(email);
            return result ? Ok(new ApiResponse(200)) : BadRequest(new ApiResponse(400));

        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            var result = await _unitwork.Auth.ResetPassword(dto);

            if (result == null)
                return NotFound(new ApiResponse(404, "User not found"));

            if (result == "Password Has Been Changed")
                return Ok(new ApiResponse(200, result));

            return BadRequest(new ApiResponse(400, result));
        }

        [HttpGet("get-user-address")]
        public async Task<IActionResult> GetUserAddress()
        {
            var address = await _unitwork.Auth.GetUserAddress(User.FindFirst(ClaimTypes.Email).Value);
            var result = _mapper.Map<shipAddressDTO>(address);
            return Ok(result);
        }

    }
}
