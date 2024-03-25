using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RollerCoaster;

public class SiteConfiguration
{
    public class JWTConfiguration
    {
        public required string Audience { get; set; }
        public required string Issuer { get; set; }
        public required string Key { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        }
    }
}