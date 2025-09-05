using Data.Models.Pessoa;
using Framework.Domain.Constants;
using Framework.Services.Interfaces.Profile;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Services.ApplicationServices.Profile
{
    public static class TokenService
    {

        public static async Task<string> GenerateTokenAsync(Usuario usuario, IAccountService accountService)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.SECRET); // Replace with your actual key

            // Basic claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nome ?? "Usuário"),
                new Claim(ClaimTypes.Role, usuario.RoleGate)
            };

            // Retrieve additional claims from the UserClaims table
            var additionalClaims = accountService.GetUserClaims(usuario.Id);
            claims.AddRange(additionalClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2), // Set your desired expiration
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.SECRET);
            var myIssuer = "https://codie.com.br";
            var myAudience = "https://codie.com.br";

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, usuario.Email),
                    new(ClaimTypes.Role, usuario.RoleGate),
                    new("id", usuario.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMonths(1),
                Issuer = myIssuer,
                Audience = myAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static int GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token?.Split(" ").Last()) as JwtSecurityToken;

            // Verifica se o token não é nulo e se tem a reivindicação (claim) de ID do usuário
            if (jwtToken == null || !jwtToken.Claims.Any(c => c.Type == "id"))
            {
                throw new ArgumentException("Token inválido ou sem ID do usuário.");
            }

            // Extrai o ID do usuário do token
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new ArgumentException("ID do usuário ausente ou inválido no token.");
            }

            return userId;
        }
    }
}
