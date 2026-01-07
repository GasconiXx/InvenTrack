using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Inventrack.API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Inventrack.API.Security
{
    public sealed class JwtOptions
    {
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public string Key { get; set; } = "";
        public int ExpiryMinutes { get; set; } = 240;
    }

    public interface IJwtTokenService
    {
        string CreateToken(Usuario user);
    }

    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _opt;

        public JwtTokenService(IOptions<JwtOptions> opt) => _opt = opt.Value;

        public string CreateToken(Usuario user)
        {
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UsuarioId.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("rolId", user.RolId.ToString())
        };

            if (user.AlmacenId.HasValue)
                claims.Add(new("almacenId", user.AlmacenId.Value.ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _opt.Issuer,
                audience: _opt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_opt.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
