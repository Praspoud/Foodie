using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Foodie.Utilities
{
    public interface IFoodieSessionAccessor
    {
        public HttpContext? HttpContext { get; }
        public IConfiguration Configuration { get; }
        public int UserId { get; }
        public string UserName { get; }
        // HttpContext GetContext(IHttpContextAccessor accessor);
    }
    public class FoodieSessionAccessor : IFoodieSessionAccessor
    {
        public HttpContext? HttpContext { get; }
        public IConfiguration Configuration { get; }
        public int UserId { get; }
        public string UserName { get; }

        public FoodieSessionAccessor(IHttpContextAccessor accessor, IConfiguration config)
        {
            this.HttpContext = accessor.HttpContext;
            this.Configuration = config;
            var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
            var token = this.HttpContext.Request
                                .Headers["Authorization"]
                                .ToString()
                                .Substring("Bearer ".Length);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            var claims = principal.Claims.ToList();


            int.TryParse(claims.Where(x => x.Type == "userId").First().Value, out int u);
            this.UserId = u;
            this.UserName = claims.FirstOrDefault(x => x.Type == "userName")?.Value;
        }
        public HttpContext GetContext(IHttpContextAccessor accessor)
        {
            return this.HttpContext;
        }
    }
}
