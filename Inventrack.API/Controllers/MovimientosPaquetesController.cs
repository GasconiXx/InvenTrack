using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventrack.API.Models;

namespace Inventrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimientosPaquetesController : ControllerBase
{
    private readonly InvenTrackContext _context;

    public MovimientosPaquetesController(InvenTrackContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovimientosPaquete>>> GetMovimientosPaquetes()
    {
        return await _context.MovimientosPaquetes.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovimientosPaquete>> GetMovimientosPaquete(int id)
    {
        var movimiento = await _context.MovimientosPaquetes.FindAsync(id);

        if (movimiento == null)
        {
            return NotFound();
        }

        return movimiento;
    }

    [HttpPost]
    public async Task<ActionResult<MovimientosPaquete>> PostMovimientosPaquete(MovimientosPaquete movimiento)
    {
        _context.MovimientosPaquetes.Add(movimiento);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMovimientosPaquete), new { id = movimiento.MovimientoId }, movimiento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovimientosPaquete(int id, MovimientosPaquete movimiento)
    {
        if (id != movimiento.MovimientoId)
        {
            return BadRequest();
        }

        _context.Entry(movimiento).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovimientosPaqueteExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovimientosPaquete(int id)
    {
        var movimiento = await _context.MovimientosPaquetes.FindAsync(id);
        if (movimiento == null)
        {
            return NotFound();
        }

        _context.MovimientosPaquetes.Remove(movimiento);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MovimientosPaqueteExists(int id)
    {
        return _context.MovimientosPaquetes.Any(e => e.MovimientoId == id);
    }
}
