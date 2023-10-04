using Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Serevices;

public class TokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
        };
        // AlcUluZfq5d0KymIecV0SBoCufQ7fKg01yVNWcTObDSbHwRcLc7hTNhhkYaaGpLY
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]??= ""));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandle = new JwtSecurityTokenHandler();

        var token = tokenHandle.CreateToken(tokenDescriptor);

        return tokenHandle.WriteToken(token);
    }
}
