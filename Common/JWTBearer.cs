using Foodie.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Foodie.Common
{
    public static class JWTBearer
    {
        public static string CreateBearerToken(EUsers user,IConfiguration config)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(config["JWT:Key"]);
            var claims = new List<Claim>
            {
                new Claim("userName", user.UserName),
                new Claim("userId", user.Id.ToString(), ClaimValueTypes.Integer),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(2000),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            tokenDescriptor.Claims = new Dictionary<string, object>();
            tokenDescriptor.Claims.Add(ClaimTypes.Name, user.UserName);
            tokenDescriptor.Claims.Add(ClaimTypes.Sid, user.Id.ToString());

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
