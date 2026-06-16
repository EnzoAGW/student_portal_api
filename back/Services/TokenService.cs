using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Model;

namespace WebApplication1.Services
{
    public class TokenService
    {


        public static object GenerateToken(Employee employee)
        {
            var key = Encoding.ASCII.GetBytes(Key.Secret);
            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]{new Claim("employeeId",employee.id.ToString())}),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            var tokenString = tokenHandler.WriteToken(token);
            return new { token = tokenString };
        }

        // Novo método que gera o token já com o papel (role) do usuário
        // Claims = informações dentro do crachá (token)
        // Ex: role = "secretaria" significa que esse usuário é da secretaria
        public static object GenerateToken(string username, string role)
        {
            var key = Encoding.ASCII.GetBytes(Key.Secret);
            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    // ClaimTypes.Role é o "cargo" do usuário — o ASP.NET entende isso automaticamente
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            return new { token = tokenHandler.WriteToken(token) };
        }

        
    }

}
