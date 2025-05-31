using Ecommorce.Core.Entities.AppUser;
using Ecommorce.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce_.infrastructure.Repositories.Services
{
    public class JwtTokenService:IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtTokenService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateAndSetToken(AppUser user)
        {
            var secret = _configuration["Token:Secret"];
            var issuer = _configuration["Token:Issuer"];

            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(issuer))
                throw new InvalidOperationException("Missing Token configuration");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            // أي claims إضافية هنا
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = issuer,
                SigningCredentials = credentials,
                NotBefore = DateTime.UtcNow
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            // ✅ خزن التوكن في الكوكي
            _httpContextAccessor.HttpContext.Response.Cookies.Append("token", jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // true لو HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(1),
                IsEssential = true
            });

            return jwt;
        }
    }
}
