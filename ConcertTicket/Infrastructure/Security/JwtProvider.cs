using ConcertTicket.Application.Interfaces;
using ConcertTicket.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConcertTicket.Infrastructure.Security
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration;

        public JwtProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(User user)
        {   
            //Definiir informacion que viaja encriptada en el token, Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("Name", user.Name)
            };

            //Obtener las credenciales secretas desde appsettings.json
            var secretKey = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret no configurado");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Crear la configuracion del token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:DurationMinutes"] ?? "60")),
                signingCredentials: creds
                );

            //Escribir el token como string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
