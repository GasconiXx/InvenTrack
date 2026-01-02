using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventrack.API.Models;

namespace Inventrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaquetesController : ControllerBase
{
    private readonly InvenTrackContext _context;

    public PaquetesController(InvenTrackContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Paquete>>> GetPaquetes()
    {
        return await _context.Paquetes.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Paquete>> GetPaquete(int id)
    {
        var paquete = await _context.Paquetes.FindAsync(id);

        if (paquete == null)
        {
            return NotFound();
        }

        return paquete;
    }

    [HttpPost]
    public async Task<ActionResult<Paquete>> PostPaquete(Paquete paquete)
    {
        _context.Paquetes.Add(paquete);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPaquete), new { id = paquete.PaqueteId }, paquete);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPaquete(int id, Paquete paquete)
    {
        if (id != paquete.PaqueteId)
        {
            return BadRequest();
        }

        _context.Entry(paquete).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PaqueteExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePaquete(int id)
    {
        var paquete = await _context.Paquetes.FindAsync(id);
        if (paquete == null)
        {
            return NotFound();
        }

        _context.Paquetes.Remove(paquete);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PaqueteExists(int id)
    {
        return _context.Paquetes.Any(e => e.PaqueteId == id);
    }
}
