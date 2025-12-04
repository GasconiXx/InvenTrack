using Inventrack.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventrack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidenciasController : ControllerBase
    {
        private readonly InvenTrackContext _context;

        public IncidenciasController(InvenTrackContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Incidencia>>> GetIncidencias()
        {
            return await _context.Incidencias.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Incidencia>> GetIncidencia(int id)
        {
            var incidencia = await _context.Incidencias.FindAsync(id);

            if (incidencia == null)
            {
                return NotFound();
            }

            return incidencia;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncidencia(int id, Incidencia incidencia)
        {
            if (id != incidencia.IncidenciaId)
            {
                return BadRequest();
            }

            _context.Entry(incidencia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncidenciaExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Incidencia>> PostIncidencia(Incidencia incidencia)
        {
            _context.Incidencias.Add(incidencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIncidencia), new { id = incidencia.IncidenciaId }, incidencia);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncidencia(int id)
        {
            var incidencia = await _context.Incidencias.FindAsync(id);
            if (incidencia == null)
            {
                return NotFound();
            }

            _context.Incidencias.Remove(incidencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IncidenciaExists(int id)
        {
            return _context.Incidencias.Any(e => e.IncidenciaId == id);
        }
    }
}
