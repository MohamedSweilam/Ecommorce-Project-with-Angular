using Ecommorce.Core.Entities.AppUser;
using Ecommorce.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class GenerateToken : IGenerateToken
{
    private readonly IConfiguration configuration;

    public GenerateToken(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public  string GetAndCreateToken(AppUser user)
    {
        var secret = configuration["Token:Secret"];
        if (string.IsNullOrEmpty(secret))
            throw new InvalidOperationException("Token:Secret is missing from configuration");

        var key = Encoding.ASCII.GetBytes(secret);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            Issuer = configuration["Token:Issuer"],
            SigningCredentials = credentials,
            NotBefore = DateTime.Now
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
