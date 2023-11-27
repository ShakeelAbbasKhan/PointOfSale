using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PointOfSale.Data;
using PointOfSale.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PointOfSale.Helper
{
    public class JWTService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTSettings _jwtSettings;


        public JWTService(UserManager<ApplicationUser> userManager,IOptions<JWTSettings> options)
        {
            _userManager = userManager;
            _jwtSettings = options.Value;
        }

        public async Task<string> GenerateTokenString(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var authClaims = new List<Claim>();

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                authClaims.AddRange(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                });

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
            }

            // var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:Secret"]));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));


            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(60),
                 //issuer: _configuration["JWTSettings:ValidIssuer"],
                 //audience: _configuration["JWTSettings:ValidAudience"],
                 issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                signingCredentials: signingCred);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
    }
}
