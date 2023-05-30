using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using StorageStrategy.Models;

namespace StorageStrategy.Utils.Services
{
    public class TokenService
    {
        public string GenerateToken(EmployeeEntity emploeyee)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = System.Text.Encoding.ASCII.GetBytes("Pegar do appsettings.json");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim(ClaimTypes.Name, emploeyee.Name.ToString()),
                    new Claim(ClaimTypes.Role, emploeyee.JobRole.ToString()),
                    new Claim("EmployeeId", emploeyee.EmployeeId.ToString()),
                    new Claim("CompanyId", emploeyee.CompanyId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}