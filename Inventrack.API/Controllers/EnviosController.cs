using Inventrack.API.Contracts.Envios;
using Inventrack.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnviosController : ControllerBase
{
    private readonly InventrackContext _context;

    public EnviosController(InventrackContext context)
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

    [HttpGet("mios")]
    public async Task<ActionResult<List<EnvioListItemDto>>> GetMisEnvios([FromQuery] int repartidorId, CancellationToken ct)
    {
        if (repartidorId <= 0) return BadRequest("repartidorId inválido.");

        
        var data = await _context.Envios
            .AsNoTracking()
            .Where(e => e.RepartidorId == repartidorId)
            .Select(e => new EnvioListItemDto
            {
                EnvioId = e.EnvioId,
                PaqueteId = e.PaqueteId,
                CodigoSeguimiento = e.Paquete.CodigoSeguimiento,
                Estado = e.Estado,
                IntentosEntrega = e.IntentosEntrega,

                DireccionDestino =
                    e.Paquete.DireccionDestino.Calle + ", " +
                    e.Paquete.DireccionDestino.Ciudad + " (" +
                    e.Paquete.DireccionDestino.CodigoPostal + ")"
            })
            .ToListAsync(ct);

        return Ok(data);
    }
}
