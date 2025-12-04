using Inventrack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventrack.API.Controllers
{
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
            var movimientosPaquete = await _context.MovimientosPaquetes.FindAsync(id);

            if (movimientosPaquete == null)
            {
                return NotFound();
            }

            return movimientosPaquete;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovimientosPaquete(int id, MovimientosPaquete movimientosPaquete)
        {
            if (id != movimientosPaquete.MovimientoId)
            {
                return BadRequest();
            }

            _context.Entry(movimientosPaquete).State = EntityState.Modified;

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

        [HttpPost]
        public async Task<ActionResult<MovimientosPaquete>> PostMovimientosPaquete(MovimientosPaquete movimientosPaquete)
        {
            _context.MovimientosPaquetes.Add(movimientosPaquete);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovimientosPaquete), new { id = movimientosPaquete.MovimientoId }, movimientosPaquete);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimientosPaquete(int id)
        {
            var movimientosPaquete = await _context.MovimientosPaquetes.FindAsync(id);
            if (movimientosPaquete == null)
            {
                return NotFound();
            }

            _context.MovimientosPaquetes.Remove(movimientosPaquete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovimientosPaqueteExists(int id)
        {
            return _context.MovimientosPaquetes.Any(e => e.MovimientoId == id);
        }
    }
}
