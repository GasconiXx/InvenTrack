using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventrack.API.Models;

namespace Inventrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly InventrackContext _context;

    public UsuariosController(InventrackContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        return await _context.Usuarios.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return usuario;
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> PostUsuario([FromBody] CreateUserRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.ContrasenaHash))
            return BadRequest("Nombre, Email y Password son obligatorios.");

        var email = request.Email.Trim().ToLowerInvariant();

        var exists = await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == email, ct);
        if (exists) return BadRequest("Ya existe un usuario con ese email.");

        var usuario = new Usuario
        {
            Nombre = request.Nombre.Trim(),
            Email = email,
            Telefono = request.Telefono?.Trim() ?? "",
            RolId = request.RolId,
            Activo = request.Activo,
            AlmacenId = request.AlmacenId,
            FechaRegistro = DateTime.UtcNow,

            ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(request.ContrasenaHash)
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync(ct);

        usuario.ContrasenaHash = "";

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.UsuarioId }, usuario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
    {
        if (id != usuario.UsuarioId)
        {
            return BadRequest();
        }

        _context.Entry(usuario).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UsuarioExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UsuarioExists(int id)
    {
        return _context.Usuarios.Any(e => e.UsuarioId == id);
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public sealed class CreateUserRequest
    {
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefono { get; set; } = "";
        public int RolId { get; set; }
        public bool Activo { get; set; } = true;
        public int? AlmacenId { get; set; }
        public string ContrasenaHash { get; set; } = "";
    }
    public sealed class UpdateUserRequest
    {
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefono { get; set; } = "";
        public int RolId { get; set; }
        public bool Activo { get; set; } = true;
        public int? AlmacenId { get; set; }
        public string? ContrasenaHash { get; set; }
    }
}
