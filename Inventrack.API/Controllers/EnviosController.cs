using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventrack.API.Models;

namespace Inventrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnviosController : ControllerBase
{
    private readonly InvenTrackContext _context;

    public EnviosController(InvenTrackContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Envio>>> GetEnvios()
    {
        return await _context.Envios.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Envio>> GetEnvio(int id)
    {
        var envio = await _context.Envios.FindAsync(id);

        if (envio == null)
        {
            return NotFound();
        }

        return envio;
    }

    [HttpPost]
    public async Task<ActionResult<Envio>> PostEnvio(Envio envio)
    {
        _context.Envios.Add(envio);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEnvio), new { id = envio.EnvioId }, envio);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutEnvio(int id, Envio envio)
    {
        if (id != envio.EnvioId)
        {
            return BadRequest();
        }

        _context.Entry(envio).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnvioExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEnvio(int id)
    {
        var envio = await _context.Envios.FindAsync(id);
        if (envio == null)
        {
            return NotFound();
        }

        _context.Envios.Remove(envio);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EnvioExists(int id)
    {
        return _context.Envios.Any(e => e.EnvioId == id);
    }
}
