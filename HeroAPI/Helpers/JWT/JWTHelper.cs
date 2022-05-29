using HeroAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HeroAPI.Helpers.JWT
{
    public static class JWTHelper
    {
        public static string CreateToken(User userModel, string tokenKey)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,userModel.Username)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
