using Inventrack.API.Contracts.Auth;
using Inventrack.API.Models;
using Inventrack.API.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly InventrackContext _db;
    private readonly IJwtTokenService _jwt;
    private readonly IWebHostEnvironment _env;

    public AuthController(InventrackContext db, IJwtTokenService jwt, IWebHostEnvironment env)
    {
        _db = db;
        _jwt = jwt;
        _env = env;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Email y contraseña son obligatorios.");

        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _db.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email, ct);

        if (user is null)
            return Unauthorized("Credenciales incorrectas.");

        if (user.Activo.HasValue && user.Activo.Value == false)
            return Unauthorized("Usuario desactivado.");

        var ok = BCrypt.Net.BCrypt.Verify(request.Password, user.ContrasenaHash);
        if (!ok)
            return Unauthorized("Credenciales incorrectas.");

        var token = _jwt.CreateToken(user);

        var response = new LoginResponse
        {
            Token = token,
            Usuario = new UsuarioDto
            {
                UsuarioId = user.UsuarioId,
                Nombre = user.Nombre,
                Email = user.Email,
                RolId = user.RolId,
                AlmacenId = user.AlmacenId,
                Activo = user.Activo
            }
        };

        return Ok(response);
    }

    [HttpPost("hash")]
    public ActionResult<string> HashPassword([FromBody] HashPasswordRequest request)
    {
        if (!_env.IsDevelopment())
            return NotFound();

        if (string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Password obligatoria.");

        const string expectedKey = "DEV_ONLY_KEY_123";
        if (!string.Equals(request.Key, expectedKey, StringComparison.Ordinal))
            return Unauthorized("Clave incorrecta.");

        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        return Ok(hash);
    }
}

