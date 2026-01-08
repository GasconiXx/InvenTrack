using Inventrack.API.Contracts.Packages;
using Inventrack.API.Contracts.Users;
using Inventrack.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly InventrackContext _context;

    public UsuarioController(InventrackContext context)
    {
        _context = context;
    }

    
    [HttpGet("all")]
    public async Task<ActionResult<List<UserListItemDto>>> GetAll(CancellationToken ct)
    {

        var data = await _context.Usuarios
            .AsNoTracking()
            .Select(p => new UserListItemDto
            {
                Nombre = p.Nombre,
                Email = p.Email,
                Rol = p.Rol.Nombre,
                Telefono = p.Telefono
            })
            .ToListAsync(ct);

        return Ok(data);
    }
}
