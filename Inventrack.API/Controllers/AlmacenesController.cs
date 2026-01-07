using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventrack.API.Models;

namespace Inventrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlmacenesController : ControllerBase
{
    private readonly InventrackContext _context;

    public AlmacenesController(InventrackContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Almacen>>> GetAlmacenes()
    {
        return await _context.Almacenes.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Almacen>> GetAlmacen(int id)
    {
        var almacen = await _context.Almacenes.FindAsync(id);

        if (almacen == null)
        {
            return NotFound();
        }

        return almacen;
    }

    [HttpPost]
    public async Task<ActionResult<Almacen>> PostAlmacen(Almacen almacen)
    {
        _context.Almacenes.Add(almacen);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAlmacen), new { id = almacen.AlmacenId }, almacen);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAlmacen(int id, Almacen almacen)
    {
        if (id != almacen.AlmacenId)
        {
            return BadRequest();
        }

        _context.Entry(almacen).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AlmacenExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlmacen(int id)
    {
        var almacen = await _context.Almacenes.FindAsync(id);
        if (almacen == null)
        {
            return NotFound();
        }

        _context.Almacenes.Remove(almacen);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AlmacenExists(int id)
    {
        return _context.Almacenes.Any(e => e.AlmacenId == id);
    }
}
