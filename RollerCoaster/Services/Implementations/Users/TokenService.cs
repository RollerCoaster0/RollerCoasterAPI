using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RollerCoaster.Services.Abstractions.Users;

namespace RollerCoaster.Services.Realisations.Users;

public class TokenService(IOptions<SiteConfiguration.JWTConfiguration> jwtConfiguration): ITokenService
{
    public string GenerateToken(int userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfiguration.Value.Key));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = jwtConfiguration.Value.Audience,
            Issuer = jwtConfiguration.Value.Issuer,
            Subject = new ClaimsIdentity(new[] { new Claim("id", userId.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(14),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}